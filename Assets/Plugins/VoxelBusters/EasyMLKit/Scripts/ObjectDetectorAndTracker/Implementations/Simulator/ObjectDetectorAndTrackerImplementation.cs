#if UNITY_EDITOR
using System.Collections.Generic;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit.Implementations.Simulator
{
    public class ObjectDetectorAndTrackerImplementation : IObjectDetectorAndTrackerImplementation
    {
        public ObjectDetectorAndTrackerImplementation(IInputSource inputSource)
        {

        }

        public void Prepare(ObjectDetectorAndTrackerOptions options, OnPrepareCompleteInternalCallback callback)
        {
            if (callback != null)
            {
                callback(null);
            }
        }

        public void Process(OnProcessUpdateInternalCallback<ObjectDetectorAndTrackerResult> callback)
        {
            if (callback != null)
            {
                callback(new ObjectDetectorAndTrackerResult(new List<DetectedObject>(), null));
            }
        }

        public void Close(OnCloseInternalCallback callback)
        {
            if (callback != null)
            {
                callback(null);
            }
        }
    }
}
#endif