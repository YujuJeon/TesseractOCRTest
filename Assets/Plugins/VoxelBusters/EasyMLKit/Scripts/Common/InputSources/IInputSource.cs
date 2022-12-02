namespace VoxelBusters.EasyMLKit
{
    /// <summary>
    /// Base interface for all kinds of input sources
    /// </summary>
    public interface IInputSource
    {
        void Close();
        float GetDisplayWidth();
        float GetDisplayHeight();
    }
}
