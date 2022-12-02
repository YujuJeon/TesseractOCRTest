
namespace VoxelBusters.EasyMLKit.Internal
{
    public interface IObjectDetectorAndTrackerImplementation
    {
        void Prepare(ObjectDetectorAndTrackerOptions options, OnPrepareCompleteInternalCallback callback);
        void Process(OnProcessUpdateInternalCallback<ObjectDetectorAndTrackerResult> callback);
        void Close(OnCloseInternalCallback callback);
    }
}
