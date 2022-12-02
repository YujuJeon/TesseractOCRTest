//
//  BarcodeScanner.h
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 25/05/22.
//

#import <Foundation/Foundation.h>
#import "IMLKitBarcodeScannerImplementation.h"
#import "IVisionInputSourceFeed.h"

NS_ASSUME_NONNULL_BEGIN

@interface MLKitBarcodeScanner : NSObject<IMLKitBarcodeScannerImplementation, IVisionInputSourceFeedDelegate>
- (id)initWithInputSource:(id<IVisionInputSourceFeed>) inputSourceFeed;
@end

NS_ASSUME_NONNULL_END
