//
//  MLKitCommon.h
//  NativeMLKitiOS
//
//  Created by Ayyappa J on 27/05/22.
//


#ifndef MLKitCommon_h
#define MLKitCommon_h
#import "NPBindingHelper.h"
#define CHAR_STRING void*

typedef struct
{
    float   x;
    float   y;
    float   width;
    float   height;
} MLKitRect;

NPBINDING DONTSTRIP MLKitRect getRect(CGRect source);
#endif
