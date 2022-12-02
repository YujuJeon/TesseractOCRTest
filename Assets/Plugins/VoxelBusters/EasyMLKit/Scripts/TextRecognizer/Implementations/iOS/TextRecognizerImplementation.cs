#if UNITY_IOS
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
    public class TextRecognizerImplementation : TextRecognizerImplementationBase
    {
        #region Private fields
        
        private bool m_initialised;
        private NativeGoogleMLKitTextRecognizer m_instance;
        private OnPrepareCompleteInternalCallback m_prepareCompleteCallback;
        private OnProcessUpdateInternalCallback<TextRecognizerResult> m_processUpdateCallback;
        private OnCloseInternalCallback m_closeCallback;
        private IInputSource m_inputSource;

        #endregion

        public TextRecognizerImplementation(IInputSource inputSource) : base(isAvailable: true)
        {
            m_inputSource = inputSource;
            m_instance = new NativeGoogleMLKitTextRecognizer(NativeInputSourceUtility.CreateInputSource(inputSource));
        }

        public override void Prepare(TextRecognizerOptions options, OnPrepareCompleteInternalCallback callback)
        {
            if (!m_initialised)
            {
                Initialise();
            }

            m_prepareCompleteCallback = callback;
            m_instance.Prepare(CreateNativeOptions(options));
        }

        public override void Process(OnProcessUpdateInternalCallback<TextRecognizerResult> callback)
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
            m_instance.SetListener(new NativeGoogleMLKitTextRecognizerListener()
            {
                onScanSuccessCallback = (NativeText nativeText, NativeSize inputSize, float inputRotation) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        Debug.Log("Received : " + nativeText.RawValue.AsString());
                        TextRecognizerResultParser parser = new TextRecognizerResultParser(nativeText, m_inputSource.GetDisplayWidth(), m_inputSource.GetDisplayHeight() ,inputSize.Width, inputSize.Height, inputRotation);
                        Callback callback = () => m_processUpdateCallback(new TextRecognizerResult(parser.GetResult(), null));
                        CallbackDispatcher.InvokeOnMainThread(callback);
                    }
                },
                onScanFailedCallback = (NativeError error) =>
                {
                    if (m_processUpdateCallback != null)
                    {
                        Callback callback = () => m_processUpdateCallback(new TextRecognizerResult(null, new Error(error.Description)));
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

        private NativeTextRecognizerScanOptions CreateNativeOptions(TextRecognizerOptions options)
        {
            NativeTextRecognizerScanOptions nativeOptions = new NativeTextRecognizerScanOptions();
            nativeOptions.Language = ConvertLanguage(options.InputLanguage);
            return nativeOptions;
        }

        private string ConvertLanguage(TextRecognizerInputLanguage inputLanguage)
        {
            switch (inputLanguage)
            {
                case TextRecognizerInputLanguage.Latin:
                    return NativeTextRecognizerInputLanguage.Latin;
                case TextRecognizerInputLanguage.Chinese:
                    return NativeTextRecognizerInputLanguage.Chinese;
                case TextRecognizerInputLanguage.Devanagari:
                    return NativeTextRecognizerInputLanguage.Devanagari;
                case TextRecognizerInputLanguage.Japanese:
                    return NativeTextRecognizerInputLanguage.Japanese;
                case TextRecognizerInputLanguage.Korean:
                    return NativeTextRecognizerInputLanguage.Korean;
                default:
                    throw new Exception("Invalid input language specified!");
            }
        }
    }
}
#endif