using VoxelBusters.CoreLibrary;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit.Implementations.Null
{
    public class ObjectDetectorAndTrackerImplementation : IObjectDetectorAndTrackerImplementation
    {
        private string NotAllowed = "Not allowed on this platform";
        public ObjectDetectorAndTrackerImplementation(IInputSource inputSource)
        {
        }

        public void Prepare(ObjectDetectorAndTrackerOptions options, OnPrepareCompleteInternalCallback callback)
        {
            if (callback != null)
            {
                callback(new Error(NotAllowed));
            }
        }

        public void Process(OnProcessUpdateInternalCallback<ObjectDetectorAndTrackerResult> callback)
        {
            if (callback != null)
            {
                callback(new ObjectDetectorAndTrackerResult(null, new Error(NotAllowed)));
            }
        }

        public void Close(OnCloseInternalCallback callback)
        {
            if (callback != null)
            {
                callback(new Error(NotAllowed));
            }
        }

    }
}
