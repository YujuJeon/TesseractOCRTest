//
//  MLKitTextRecognizerListenerWrapper.m
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 01/06/22.
//

#import "MLKitTextRecognizerListenerWrapper.h"

@interface MLKitTextRecognizerListenerWrapper ()
@property (nonatomic, assign) void* tag;
@end

@implementation MLKitTextRecognizerListenerWrapper
@synthesize tag;

-(id) initWithTag:(void*) tag
{
    self = [super init];
    self.tag = tag;
    
    return self;
}


- (void)onPrepareFailed:(nonnull NSError *)error {
    if(_prepareFailedCallback != NULL)
    {
        _prepareFailedCallback(self.tag,NPCreateError(0, [error localizedDescription]));
    }
}

- (void)onPrepareSuccess {
    if(_prepareSuccessCallback != NULL)
    {
        _prepareSuccessCallback(self.tag);
    }
}

- (void)onScanFailed:(nonnull NSError *)error {
    if(_scanFailedCallback != NULL)
    {
        _scanFailedCallback(self.tag, NPCreateError(0, [error localizedDescription]));
    }
}

- (void)onScanSuccess:(nonnull MLKitTextRecognizerResult*)result {
    if(_scanSuccessCallback != NULL)
    {
        MLKText* source = result.text;
        //NSLog(@"Raw Value %@", rawValue.text);
        MLKitText* text = (MLKitText*)malloc(sizeof(MLKitText));
        
        convertToMLKitText(source, text);
        //NSLog(@"Converted Value %@", text);
        //printf("%s", text->rawValue);

        _scanSuccessCallback(self.tag, *text, NPCreateSizeFromCGSize(result.inputSize), result.inputRotation); //@@ *text not copying properly when NPArray is used. Always needs to use NPArrayProxy when using indirection operator when passing to unity.
    }
}

@end
