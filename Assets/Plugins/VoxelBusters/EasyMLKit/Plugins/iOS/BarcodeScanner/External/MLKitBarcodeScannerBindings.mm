//
//  MLKitBarcodeScannerExternalBindings.m
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 01/06/22.
//

#import "MLKitBarcodeScannerBindings.h"
#import "MLKitBarcodeScannerListenerWrapper.h"
#import "MLKitBarcodeScanner.h"

// C bindings
NPBINDING DONTSTRIP void* MLKit_BarcodeScanner_InitWithInputSource(void* inputSourcePtr)
{
    return (__bridge_retained void*)[[MLKitBarcodeScanner alloc] initWithInputSource:(__bridge id<IVisionInputSourceFeed>)inputSourcePtr];
}

NPBINDING DONTSTRIP void MLKit_BarcodeScanner_SetListener( void* barcodeScannerPtr,
                              void* listenerTag,
                              BarcodeScannerPrepareSuccessNativeCallback prepareSuccessCallback,
                              BarcodeScannerPrepareFailedNativeCallback prepareFailedCallback,
                              BarcodeScannerScanSuccessNativeCallback scanSuccessCallback,
                              BarcodeScannerScanFailedNativeCallback scanFailedCallback)
{
    MLKitBarcodeScanner*     scanner  = (__bridge MLKitBarcodeScanner*)barcodeScannerPtr;
    MLKitBarcodeScannerListenerWrapper *wrapper = [[MLKitBarcodeScannerListenerWrapper alloc] initWithTag: listenerTag];
    wrapper.prepareSuccessCallback = prepareSuccessCallback;
    wrapper.prepareFailedCallback = prepareFailedCallback;
    wrapper.scanSuccessCallback = scanSuccessCallback;
    wrapper.scanFailedCallback = scanFailedCallback;
    [scanner setListener:wrapper];
}

NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Prepare(void* barcodeScannerPtr, void* barcodeScanOptionsPtr)
{
    MLKitBarcodeScanner*         scanner                 = (__bridge MLKitBarcodeScanner*)barcodeScannerPtr;
    MLKitBarcodeScanOptions*      barcodeScanOptions      = (__bridge MLKitBarcodeScanOptions*)barcodeScanOptionsPtr;
    [scanner prepare:barcodeScanOptions];
}

NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Process(void* barcodeScannerPtr)
{
    MLKitBarcodeScanner*         scanner  = (__bridge MLKitBarcodeScanner*)barcodeScannerPtr;
    [scanner process];
}

NPBINDING DONTSTRIP void MLKit_BarcodeScanner_Close(void* barcodeScannerPtr)
{
    MLKitBarcodeScanner*         scanner = (__bridge MLKitBarcodeScanner*)barcodeScannerPtr;
    [scanner close];
}
