using System;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;
using VoxelBusters.EasyMLKit.Internal;
using TMPro;
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
using UnityEngine.XR.ARFoundation;
using VoxelBusters.EasyMLKit.Demo;
#endif

// internal namespace
namespace VoxelBusters.EasyMLKit.Demo
{
    public class TextRecognizerDemo : DemoActionPanelBase<TextRecognizerDemoAction, TextRecognizerDemoActionType>
    {
        #region Fields
        private bool m_autoClose;
        [SerializeField] private TextMeshProUGUI txtDisplay;
        [SerializeField] private ConsoleRect consoleRect;
        [SerializeField] private TextMeshProUGUI clickedText;
        
        #endregion


        #region Base class methods

        protected override void OnActionSelectInternal(TextRecognizerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case TextRecognizerDemoActionType.Capture:
                    m_autoClose = true;
                    ScanTextFromImage();
                    break;
                case TextRecognizerDemoActionType.Scan:
                    m_autoClose = false;
                    ScanTextFromARCamera();
                    break;                                   
                case TextRecognizerDemoActionType.Close:
                    m_autoClose = true;
                    OnCloseApplication();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Usecases methods

        private void ScanTextFromImage()
        {
            IInputSource inputSource = CreateImageInputSource(DemoResources.GetRandomImage());
            TextRecognizerOptions options = CreateTextRecognizerOptions();
            Scan(inputSource, options);
        }

        public void ScanTextFromARCamera()
        {
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
            IInputSource inputSource = CreateARCameraInputSource();
            TextRecognizerOptions options = CreateTextRecognizerOptions();
            Scan(inputSource, options);
#endif
        }
                
        public void OnCloseApplication()
        {
            consoleRect.strArr.Clear();
            txtDisplay.text = "";

            IInputSource inputSource = CreateARCameraInputSource();
            TextRecognizerOptions options = CreateTextRecognizerOptions();
            StopScan(inputSource, options);
        }

        #endregion

        #region Utility methods

        private IInputSource CreateImageInputSource(Texture2D texture)
        {
            return new ImageInputSource(texture);
        }
               
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
        private IInputSource CreateARCameraInputSource()
        {
            ARSession arSession = FindObjectOfType<ARSession>();
            ARCameraManager arCameraManager = FindObjectOfType<ARCameraManager>();
            IInputSource inputSource = new ARFoundationCameraInputSource(arSession, arCameraManager);
            return inputSource;
        }
#endif

        private TextRecognizerOptions CreateTextRecognizerOptions()
        {
            TextRecognizerOptions.Builder builder = new TextRecognizerOptions.Builder();
            builder.SetInputLanguage(TextRecognizerInputLanguage.Latin);
            return builder.Build();
        }

        private void StopScan(IInputSource inputSource, TextRecognizerOptions options)
        {
            TextRecognizer scanner = new TextRecognizer(inputSource);            
            scanner.Prepare(options, OnStopPrepareComplete);
        }

        private void Scan(IInputSource inputSource, TextRecognizerOptions options)
        {
            TextRecognizer scanner = new TextRecognizer(inputSource);
            Debug.Log("Starting prepare...");
            scanner.Prepare(options, OnPrepareComplete);
        }

        private void OnStopPrepareComplete(TextRecognizer scanner, Error error)
        {
            scanner.Process(DisableTextGroupBox);
        }

        private void OnPrepareComplete(TextRecognizer scanner, Error error)
        {
            Debug.Log("Prepare complete..." + error);
            if (error == null)
            {
                Log("Prepare completed successfully!");
                scanner.Process(OnProcessUpdate);
            }
            else
            {
                Log("Failed preparing Text Recognizer : " + error.Description);
            }
        }

        private void OnProcessUpdate(TextRecognizer scanner, TextRecognizerResult result)
        {
            if (!result.HasError())
            {
                Log(string.Format("{0}", result.TextGroup.Text), true);
                TextGroup textGroup = result.TextGroup;

                if (textGroup != null)
                {
                    ObjectOverlayController.Instance.ClearAll();
                    if (textGroup.Blocks != null)
                    {
                        foreach (TextGroup.Block each in textGroup.Blocks)
                        {
                            ObjectOverlayController.Instance.ShowOverlay(each.BoundingBox, string.Format("{0}", each.Text));
                            
                            //TODO://
                            //check what is overlay rect gameobject and add onclick listener to grab selected Text

                            //clickedText.text = ObjectOverlayController.Instance.OverlayTextClicked(each.Text);
                        }
                    }

                    if(m_autoClose)
                    {
                        scanner.Close(null);
                    }
                }

                //display array list
                for (int i = 0; i < consoleRect.strArr.Count; i++)
                {
                    txtDisplay.text = consoleRect.strArr[i].ToString();
                }


            }
            else
            {
                Log("Text Recognizer failed processing : " + result.Error.Description, false);
            }
        }

        private void DisableTextGroupBox(TextRecognizer scanner, TextRecognizerResult result)
        {
            TextGroup textGroup = result.TextGroup;
            ObjectOverlayController.Instance.ClearAll();
            scanner.Close(null);
        }

        #endregion
    }
}
