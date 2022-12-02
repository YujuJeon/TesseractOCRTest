#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EasyMLKit.Implementations.iOS.Internal
{
    internal class NativeGoogleMLKitBarcodeScanner
    {
        #region Native Bindings

        [DllImport("__Internal")]
        public static extern IntPtr MLKit_BarcodeScanner_InitWithInputSource(IntPtr inputSourcePtr);

        [DllImport("__Internal")]
        public static extern void MLKit_BarcodeScanner_SetListener(IntPtr barcodeScannerPtr,
                                      IntPtr listenerTag,
                                      NativeGoogleMLKitBarcodeScannerListener.BarcodeScannerPrepareSuccessNativeCallback prepareSuccessCallback,
                                      NativeGoogleMLKitBarcodeScannerListener.BarcodeScannerPrepareFailedNativeCallback prepareFailedCallback,
                                      NativeGoogleMLKitBarcodeScannerListener.BarcodeScannerScanSuccessNativeCallback scanSuccessCallback,
                                      NativeGoogleMLKitBarcodeScannerListener.BarcodeScannerScanFailedNativeCallback scanFailedCallback);
        [DllImport("__Internal")]
        public static extern void MLKit_BarcodeScanner_Prepare(IntPtr barcodeScannerPtr, IntPtr barcodeScanOptionsPtr);

        [DllImport("__Internal")]
        public static extern void MLKit_BarcodeScanner_Process(IntPtr barcodeScannerPtr);

        [DllImport("__Internal")]
        public static extern void MLKit_BarcodeScanner_Close(IntPtr barcodeScannerPtr);


        #endregion

        #region Fields

        IntPtr m_nativeHandle;

        #endregion

        #region Constructor

        public NativeGoogleMLKitBarcodeScanner(IntPtr inputSource)
        {
            m_nativeHandle = MLKit_BarcodeScanner_InitWithInputSource(inputSource);
        }

        #endregion

        #region Public methods

        public void Close()
        {
            MLKit_BarcodeScanner_Close(m_nativeHandle);
        }

        public void Prepare(NativeBarcodeScanOptions options)
        {
            MLKit_BarcodeScanner_Prepare(m_nativeHandle, options.NativeHandle);
        }
        public void Process()
        {
            MLKit_BarcodeScanner_Process(m_nativeHandle);
        }
        public void SetListener(NativeGoogleMLKitBarcodeScannerListener listener)
        {
            MLKit_BarcodeScanner_SetListener(m_nativeHandle, listener.NativeHandle, NativeGoogleMLKitBarcodeScannerListener.PrepareSuccessNativeCallback,
                                                    NativeGoogleMLKitBarcodeScannerListener.PrepareFailedNativeCallback,
                                                    NativeGoogleMLKitBarcodeScannerListener.ScanSuccessNativeCallback,
                                                    NativeGoogleMLKitBarcodeScannerListener.ScanFailedNativeCallback);
        }

        #endregion
    }
}
#endif