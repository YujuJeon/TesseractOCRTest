//
//  MLKitTextRecognizerExternalBindings.m
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 01/06/22.
//

#import "MLKitTextRecognizerBindings.h"
#import "MLKitTextRecognizerListenerWrapper.h"
#import "MLKitTextRecognizer.h"

// C bindings
NPBINDING DONTSTRIP void* MLKit_TextRecognizer_InitWithInputSource(void* inputSourcePtr)
{
    return (__bridge_retained void*)[[MLKitTextRecognizer alloc] initWithInputSource:(__bridge id<IVisionInputSourceFeed>)inputSourcePtr];
}

NPBINDING DONTSTRIP void MLKit_TextRecognizer_SetListener( void* textRecognizerPtr,
                              void* listenerTag,
                              TextRecognizerPrepareSuccessNativeCallback prepareSuccessCallback,
                              TextRecognizerPrepareFailedNativeCallback prepareFailedCallback,
                              TextRecognizerSuccessNativeCallback scanSuccessCallback,
                              TextRecognizerFailedNativeCallback scanFailedCallback)
{
    MLKitTextRecognizer*     recognizer  = (__bridge MLKitTextRecognizer*)textRecognizerPtr;
    MLKitTextRecognizerListenerWrapper *wrapper = [[MLKitTextRecognizerListenerWrapper alloc] initWithTag: listenerTag];
    wrapper.prepareSuccessCallback = prepareSuccessCallback;
    wrapper.prepareFailedCallback = prepareFailedCallback;
    wrapper.scanSuccessCallback = scanSuccessCallback;
    wrapper.scanFailedCallback = scanFailedCallback;
    [recognizer setListener:wrapper];
}

NPBINDING DONTSTRIP void MLKit_TextRecognizer_Prepare(void* textRecognizerPtr, void* textRecognizerScanOptionsPtr)
{
    MLKitTextRecognizer*         recognizer                 = (__bridge MLKitTextRecognizer*)textRecognizerPtr;
    MLKitTextRecognizerScanOptions*      textRecognizerScanOptions      = (__bridge MLKitTextRecognizerScanOptions*)textRecognizerScanOptionsPtr;
    [recognizer prepare:textRecognizerScanOptions];
}

NPBINDING DONTSTRIP void MLKit_TextRecognizer_Process(void* textRecognizerPtr)
{
    MLKitTextRecognizer*         recognizer  = (__bridge MLKitTextRecognizer*)textRecognizerPtr;
    [recognizer process];
}

NPBINDING DONTSTRIP void MLKit_TextRecognizer_Close(void* textRecognizerPtr)
{
    MLKitTextRecognizer*         recognizer = (__bridge MLKitTextRecognizer*)textRecognizerPtr;
    [recognizer close];
}
