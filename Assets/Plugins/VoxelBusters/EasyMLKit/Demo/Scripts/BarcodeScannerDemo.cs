using System;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;
using VoxelBusters.EasyMLKit.Internal;
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
using UnityEngine.XR.ARFoundation;
#endif

// internal namespace
namespace VoxelBusters.EasyMLKit.Demo
{
    public class BarcodeScannerDemo : DemoActionPanelBase<BarcodeScannerDemoAction, BarcodeScannerDemoActionType>
    {
        #region Fields

        private bool m_autoClose;

        #endregion

        #region Base class methods

        protected override void OnActionSelectInternal(BarcodeScannerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case BarcodeScannerDemoActionType.ScanBarcodeFromImage:
                    m_autoClose = true;
                    ScanBarcodeFromImage();
                    break;

                case BarcodeScannerDemoActionType.ScanBarcodeFromLiveCamera:
                    m_autoClose = true;
                    ScanBarcodeFromLiveCamera();
                    break;

                case BarcodeScannerDemoActionType.ScanBarcodeFromARCamera:
                    m_autoClose = false;
                    ScanBarcodeFromARCamera();
                    break;

                case BarcodeScannerDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kBarcodeScanner);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Usecases methods

        private void ScanBarcodeFromImage()
        {
            IInputSource inputSource = CreateImageInputSource(DemoResources.GetRandomImage());
            BarcodeScannerOptions options = CreateBarcodeScannerOptions();
            Scan(inputSource, options);
        }

        private void ScanBarcodeFromLiveCamera()
        {
            IInputSource inputSource = CreateLiveCameraInputSource();
            BarcodeScannerOptions options = CreateBarcodeScannerOptions();
            Scan(inputSource, options);
        }

        private void ScanBarcodeFromARCamera()
        {
#if EASY_ML_KIT_SUPPORT_AR_FOUNDATION
            IInputSource inputSource = CreateARCameraInputSource();//Now we use live camera as input source
            BarcodeScannerOptions options = CreateBarcodeScannerOptions();
            Scan(inputSource, options);
#endif
        }

        #endregion

        #region Utility methods

        private IInputSource CreateImageInputSource(Texture2D texture)
        {
            return new ImageInputSource(texture);
        }

        private IInputSource CreateLiveCameraInputSource()
        {
            LiveCameraInputSource inputSource = new LiveCameraInputSource();
            inputSource.EnableFlash = false;
            inputSource.IsFrontFacing = false;

            return inputSource;
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

        private BarcodeScannerOptions CreateBarcodeScannerOptions()
        {
            BarcodeScannerOptions.Builder builder = new BarcodeScannerOptions.Builder();
            builder.SetScannableFormats(BarcodeFormat.ALL);
            return builder.Build();
        }

        private void Scan(IInputSource inputSource, BarcodeScannerOptions options)
        {
            BarcodeScanner scanner = new BarcodeScanner(inputSource);
            Debug.Log("Starting prepare...");
            scanner.Prepare(options, OnPrepareComplete);
        }

        private void OnPrepareComplete(BarcodeScanner scanner, Error error)
        {
            Debug.Log("Prepare complete..." + error);
            if (error == null)
            {
                Log("Prepare completed successfully!");
                scanner.Process(OnProcessUpdate);
            }
            else
            {
                Log("Failed preparing Barcode scanner : " + error.Description);
            }
        }

        private void OnProcessUpdate(BarcodeScanner scanner, BarcodeScannerResult result)
        {
            if (!result.HasError())
            {
                if (result.Barcodes != null)
                {
                    foreach (Barcode each in result.Barcodes)
                    {
                        Log(string.Format("Format : {0}, Value Type : {1}, Raw Value : {2}, Bounding Box : {3}", each.Format, each.ValueType, each.RawValue, each.BoundingBox), false);
                    }
                    if (result.Barcodes.Count > 0)
                    {
                        ObjectOverlayController.Instance.ClearAll();
                        foreach (Barcode each in result.Barcodes)
                        {
                            ObjectOverlayController.Instance.ShowOverlay(each.BoundingBox, each.DisplayValue);
                        }

                        if (m_autoClose)
                        {
                            scanner.Close(null);
                        }
                    }
                }
            }
            else
            {
                Log("Barcode scanner failed processing : " + result.Error.Description, false);
            }
        }

        #endregion
    }
}
