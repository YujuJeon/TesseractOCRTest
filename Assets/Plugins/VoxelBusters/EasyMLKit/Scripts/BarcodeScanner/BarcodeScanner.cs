using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit
{
    /// <summary>
    /// Scan different types of barcodes by passing different input sources.
    /// </summary>
    public class BarcodeScanner
    {
        public BarcodeScannerOptions Options
        {
            get;
            private set;
        }

        private IBarcodeScannerImplementation m_implementation;


        #region Static properties

        public static BarcodeScannerUnitySettings UnitySettings
        {
            get
            {
                return EasyMLKitSettings.Instance.BarcodeScannerSettings;
            }
        }

        #endregion

        /// <summary>
        /// Pass input source to consider for creating a BarcodeScanner instance.
        /// </summary>
        /// <param name="inputSource"></param>
        public BarcodeScanner(IInputSource inputSource)
        {
            try
            {

#if !UNITY_EDITOR && UNITY_ANDROID
                m_implementation = new Implementations.Android.BarcodeScannerImplementation(inputSource);
#else
                m_implementation = NativeFeatureActivator.CreateInterface<IBarcodeScannerImplementation>(ImplementationBlueprint.BarcodeScanner, UnitySettings.IsEnabled, inputSource);
#endif
            }
            catch(Exception e)
            {
                DebugLogger.LogError($"[{Application.platform}] Failed creating required implementation with exception : " + e);
            }

            if(m_implementation == null)
            {
                DebugLogger.LogWarning($"[{Application.platform}] Using null implementation as no platform specific implementation found");
                m_implementation = new Implementations.Null.BarcodeScannerImplementation(inputSource);
            }
        }

        /// <summary>
        /// Prepare with options. Callback will be called once prepare is complete.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="callback"></param>
        public void Prepare(BarcodeScannerOptions options, OnPrepareCompleteCallback<BarcodeScanner> callback)
        {
            Options = options;
            m_implementation.Prepare(options, (error) => callback?.Invoke(this, error));
        }

        /// <summary>
        /// Start processing the input with the provided options. Callback will be called with BarcodeScannerResult once processing has an update/done 
        /// </summary>
        /// <param name="callback"></param>
        public void Process(OnProcessUpdateCallback<BarcodeScanner, BarcodeScannerResult> callback)
        {
            m_implementation.Process((result) => callback?.Invoke(this, result));
        }

        /// <summary>
        /// Shutdown once the processing is done to release resources
        /// </summary>
        /// <param name="callback"></param>
        public void Close(OnCloseCallback<BarcodeScanner> callback)
        {
            m_implementation.Close((error) => callback?.Invoke(this, error));
        }
    }
}

