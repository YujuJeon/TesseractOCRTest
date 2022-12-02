﻿#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EasyMLKit.Implementations.iOS.Internal;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit.Implementations.iOS
{
    public class BarcodeScannerImplementation : BarcodeScannerImplementationBase
    {
        #region Private fields
        
        private bool m_initialised;
        private NativeGoogleMLKitBarcodeScanner m_instance;
        private OnPrepareCompleteInternalCallback m_prepareCompleteCallback;
        private OnProcessUpdateInternalCallback<BarcodeScannerResult> m_processUpdateCallback;
        private OnCloseInternalCallback m_closeCallback;
        private IInputSource m_inputSource;

        #endregion

        public BarcodeScannerImplementation(IInputSource inputSource) : base(isAvailable: true)
        {
            m_inputSource = inputSource;
            m_instance = new NativeGoogleMLKitBarcodeScanner(NativeInputSourceUtility.CreateInputSource(inputSource));
        }

        public override void Prepare(BarcodeScannerOptions options, OnPrepareCompleteInternalCallback callback)
        {
            if (!m_initialised)
            {
                Initialise();
            }

            m_prepareCompleteCallback = callback;
            m_instance.Prepare(CreateNativeOptions(options));
        }

        public override void Process(OnProcessUpdateInternalCallback<BarcodeScannerResult> callback)
        {
            m_processUpdateCallback = callback;
            m_instance.Process();
        }

        public override void Close(OnCloseInternalCallback callback)
        {
            m_closeCallback = callback;
            m_instance.Close();

            if(m_closeCallback != null)
            {
                m_closeCallback(null);
            }
        }

        private void Initialise()
        {
            SetupListener();
            m_initialised = true;
        }

        
        private void SetupListener()
        {
            m_instance.SetListener(new NativeGoogleMLKitBarcodeScannerListener()
            {
                onScanSuccessCallback = (NativeArray nativeBarcodes, NativeSize inputSize, float inputRotation) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        BarcodeResultParser parser = new BarcodeResultParser(nativeBarcodes, m_inputSource.GetDisplayWidth(), m_inputSource.GetDisplayHeight() ,inputSize.Width, inputSize.Height, inputRotation);
                        Callback callback = () => m_processUpdateCallback(new BarcodeScannerResult(parser.GetResult(), null));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                },
                onScanFailedCallback = (NativeError error) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        Callback callback = () => m_processUpdateCallback(new BarcodeScannerResult(null, new Error(error.Description)));
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
                onPrepareFailedCallback = (NativeError error) =>
                {
                    if (m_prepareCompleteCallback != null)
                    {
                        Callback callback = () => m_prepareCompleteCallback(new Error(error.Description));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                }
            });
        }

        private int GetAllowedFormats(BarcodeFormat scannableFormats)
        {
            return (int)scannableFormats;//Both native format int values and c# values match. So we can pass directly.
        }

        private NativeBarcodeScanOptions CreateNativeOptions(BarcodeScannerOptions options)
        {
            NativeBarcodeScanOptions nativeOptions = new NativeBarcodeScanOptions();
            nativeOptions.AllowedFormats = GetAllowedFormats(options.ScannableFormats);
            return nativeOptions;
        }
    }
}
#endif