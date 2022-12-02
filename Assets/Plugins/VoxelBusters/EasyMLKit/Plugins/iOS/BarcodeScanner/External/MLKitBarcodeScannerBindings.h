//
//  MLKitBarcodeScannerExternalBindings.h
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 01/06/22.
//

#import "MLKitBarcodeScannerListenerWrapper.h"
#import "NPKit.h"

NS_ASSUME_NONNULL_BEGIN

NPBINDING DONTSTRIP void* MLKit_BarcodeScanner_InitWithInputSource(void* inputSourcePtr);
NPBINDING DONTSTRIP void MLKit_BarcodeScanner_SetListener( void* barcodeScannerPtr,
                              void* listenerTag,
                              BarcodeScannerPrepareSuccessNativeCallback prepareSuccessCallback,
                              BarcodeScannerPrepareFailedNativeCallback prepareFailedCallback,
                              BarcodeScannerScanSuccessNativeCallback scanSuccessCallback,
                                      BarcodeScannerScanFailedNativeCallback scanFailedCallback);
NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Prepare(void* barcodeScannerPtr, void* barcodeScanOptionsPtr);
NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Process(void* barcodeScannerPtr);
NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Close(void* barcodeScannerPtr);

NS_ASSUME_NONNULL_END
