using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using TMPro;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;
using UnityEngine.UI;
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
using VoxelBusters.EasyMLKit;
#endif

namespace VoxelBusters.EasyMLKit.Demo
{
    public class TextRecognitionManager : MonoBehaviour
    {
        [SerializeField] private GameObject txtDisplay;
        [SerializeField] private ConsoleRect consoleRect;
        [SerializeField] private TextRecognizerDemo demo;
        [SerializeField] private GameObject btnScan;
        [SerializeField] private GameObject btnCapture;
        [SerializeField] private GameObject btnClose;

        private void Awake()
        {
            btnCapture.SetActive(false);
            txtDisplay.SetActive(false);
            btnClose.SetActive(false);
        }

        public void OnClickScan()
        {
            btnCapture.SetActive(true);
            txtDisplay.SetActive(true);
            btnClose.SetActive(true);
           // demo.ScanTextFromARCamera();
           // ObjectOverlayController.Instance.ShowOverlay();
        }

        public void OnClickCapture()
        {

        }

        public void OnClickClose()
        {
            btnCapture.SetActive(false);
            txtDisplay.SetActive(false);
            btnClose.SetActive(false);
            //txtDisplay.transform.GetComponent<TextMeshProUGUI>().text = "";
            //consoleRect.strArr = null;
            //demo.OnCloseApplication();
        }

        //public void DisplayMessage()
        //{
        //    for (int i = 0; i < consoleRect.strArr.Count; i++)
        //    {
        //        txtDisplay.text = consoleRect.strArr[i].ToString();
        //    }
        //}


    }
}