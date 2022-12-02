#if UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EasyMLKit.NativePlugins.Android;

namespace VoxelBusters.EasyMLKit.Implementations.Android
{
    using Internal;
    using VoxelBusters.EasyMLKit.Internal;

    public class ObjectDetectorAndTrackerImplementation : NativeFeatureBase, IObjectDetectorAndTrackerImplementation
    {
#region Private fields

        private bool m_initialised;
        private ObjectDetectorAndTrackerOptions m_options;
        private NativeGoogleMLKitObjectDetectionAndTracking m_instance;
        private OnPrepareCompleteInternalCallback m_prepareCompleteCallback;
        private OnProcessUpdateInternalCallback<ObjectDetectorAndTrackerResult> m_processUpdateCallback;
        private OnCloseInternalCallback m_closeCallback;

#endregion

        public ObjectDetectorAndTrackerImplementation(IInputSource inputSource) : base(inputSource)
        {
            m_instance = new NativeGoogleMLKitObjectDetectionAndTracking(NativeUnityPluginUtility.GetContext(), GetNativeInputImageProducer(inputSource));
        }

        public void Prepare(ObjectDetectorAndTrackerOptions options, OnPrepareCompleteInternalCallback callback)
        {
            m_options = options;

            if (!m_initialised)
                Initialise();

            m_prepareCompleteCallback = callback;
            m_instance.Prepare(CreateNativeOptions(m_options));
        }

        public void Process(OnProcessUpdateInternalCallback<ObjectDetectorAndTrackerResult> callback)
        {
            m_processUpdateCallback = callback;
            m_instance.Process();
        }

        public void Close(OnCloseInternalCallback callback)
        {
            m_closeCallback = callback;
            Close();
            m_instance.Close();
        }

        private void Initialise()
        {
            SetupListener();
        }

        private void SetupListener()
        {
            m_instance.SetListener(new NativeGoogleMLKitObjectDetectionAndTrackingListener()
            {
                onDetectionSuccessCallback = (NativeList<NativeDetectedObject> nativeDetectedObjects, NativeInputImageInfo inputDimensions) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        List<DetectedObject> detectedObjects = GetDetectedObjects(nativeDetectedObjects.Get(), inputDimensions);
                        Callback callback = () => m_processUpdateCallback(new ObjectDetectorAndTrackerResult(detectedObjects, null));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                },
                onDetectionFailedCallback = (NativeException exception) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        Callback callback = () => m_processUpdateCallback(new ObjectDetectorAndTrackerResult(null, new Error(exception.GetMessage())));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                },
                onPrepareSuccessCallback = () =>
                {
                    if (m_prepareCompleteCallback != null)
                    {
                        Callback callback = () => m_prepareCompleteCallback(null);
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                },
                onPrepareFailedCallback = (NativeException exception) =>
                {
                    if (m_prepareCompleteCallback != null)
                    {
                        Callback callback = () => m_prepareCompleteCallback(new Error(exception.GetMessage()));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                }
            });
        }

        private NativeObjectDetectionAndTrackingOptions CreateNativeOptions(ObjectDetectorAndTrackerOptions options)
        {
            NativeObjectDetectionAndTrackingOptions nativeOptions = new NativeObjectDetectionAndTrackingOptions();
            nativeOptions.SetEnableClassification(options.EnableClassification);
            nativeOptions.SetClassificationConfidenceThreshold(options.ClassificationConfidenceThreshold);
            nativeOptions.SetEnableMultiObjectDetection(options.EnableMultipleObjectDetection);
            nativeOptions.SetModelPath(options.CustomModelPath.ToString());
            return nativeOptions;
        }

        private List<DetectedObject> GetDetectedObjects(List<NativeDetectedObject> nativeDetectedObjects, NativeInputImageInfo inputDimensions)
        {
            List<DetectedObject> detectedObjects = new List<DetectedObject>();

            foreach(NativeDetectedObject nativeDetectedObject in nativeDetectedObjects)
            {
                DetectedObject detectedObject = ConvertFromNativeDetectedObject(nativeDetectedObject, inputDimensions);
                detectedObjects.Add(detectedObject);
            }

            return detectedObjects;
        }

        private DetectedObject ConvertFromNativeDetectedObject(NativeDetectedObject source, NativeInputImageInfo inputDimensions)
        {
            NativeInteger nativeTrackingId = source.GetTrackingId();
            int? trackingId = nativeTrackingId?.GetIntValue();

            List<DetectedObject.Label> labels = GetLabels(source.GetLabels().Get());
            Rect boundingBox = GetRect(source.GetBoundingBox(), inputDimensions);

            return new DetectedObject(boundingBox, labels, trackingId);
        }

        private List<DetectedObject.Label> GetLabels(List<NativeLabel> source)
        {
            List<DetectedObject.Label> labels = new List<DetectedObject.Label>();
            foreach(NativeLabel each in source)
            {
                DetectedObject.Label label = new DetectedObject.Label(each.GetIndex(), each.GetText(), each.GetConfidence());
                labels.Add(label);
            }

            return labels;
        }

        private Rect GetRect(NativeRect nativeRect, NativeInputImageInfo imageInfo)
        {
            Rect rawRect = new Rect(nativeRect.Left, nativeRect.Top, nativeRect.Right - nativeRect.Left, nativeRect.Bottom - nativeRect.Top);
            return InputSourceUtility.TransformRectToUserSpace(rawRect, m_inputSource.GetDisplayWidth(), m_inputSource.GetDisplayHeight(), imageInfo.GetWidth(), imageInfo.GetHeight(), imageInfo.GetRotation());
        }
    }
}
#endif