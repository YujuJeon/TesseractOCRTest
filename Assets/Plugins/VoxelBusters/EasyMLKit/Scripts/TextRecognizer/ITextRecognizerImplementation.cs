﻿
namespace VoxelBusters.EasyMLKit
{
    using Internal;
    using VoxelBusters.CoreLibrary.NativePlugins;

    public interface ITextRecognizerImplementation : INativeFeatureInterface
    {
        void Prepare(TextRecognizerOptions options, OnPrepareCompleteInternalCallback callback);
        void Process(OnProcessUpdateInternalCallback<TextRecognizerResult> callback);
        void Close(OnCloseInternalCallback callback);
    }
}