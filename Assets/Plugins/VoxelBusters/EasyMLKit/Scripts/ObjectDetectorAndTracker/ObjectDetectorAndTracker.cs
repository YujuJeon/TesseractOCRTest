using System;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit
{
    /// <summary>
    /// Feature to detect an object and track it
    /// </summary>
    public class ObjectDetectorAndTracker
    {
        public ObjectDetectorAndTrackerOptions Options
        {
            get;
            private set;
        }

        private IObjectDetectorAndTrackerImplementation m_implementation;

        /// <summary>
        /// Pass an input source to create an instance
        /// </summary>
        /// <param name="inputSource"></param>
        public ObjectDetectorAndTracker(IInputSource inputSource)
        {
            try
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                m_implementation    = new Implementations.Android.ObjectDetectorAndTrackerImplementation(inputSource);
#elif UNITY_EDITOR
                m_implementation = new Implementations.Simulator.ObjectDetectorAndTrackerImplementation(inputSource);
#endif
            }
            catch (Exception e)
            {
                DebugLogger.LogError("Failed creating required implementation with exception : " + e);
                m_implementation = new Implementations.Null.ObjectDetectorAndTrackerImplementation(inputSource);
            }
        }

        /// <summary>
        /// Prepare with options and callback gets called once prepare is completed.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="callback"></param>
        public void Prepare(ObjectDetectorAndTrackerOptions options, OnPrepareCompleteCallback<ObjectDetectorAndTracker> callback)
        {
            Options = options;
            m_implementation.Prepare(options, (error) => callback?.Invoke(this, error));
        }

        /// <summary>
        /// Process the input source with the provided options. Callback gets called with a result once processing of current input frame is done.
        /// </summary>
        /// <param name="callback"></param>
        public void Process(OnProcessUpdateCallback<ObjectDetectorAndTracker, ObjectDetectorAndTrackerResult> callback)
        {
            m_implementation.Process((result) => callback?.Invoke(this, result));
        }

        /// <summary>
        /// Shutdown to release resources.
        /// </summary>
        /// <param name="callback"></param>
        public void Close(OnCloseCallback<ObjectDetectorAndTracker> callback)
        {
            m_implementation.Close((error) => callback?.Invoke(this, error));
        }
    }
}

