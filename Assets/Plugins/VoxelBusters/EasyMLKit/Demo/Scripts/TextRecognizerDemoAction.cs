﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EasyMLKit.Demo
{
    public enum TextRecognizerDemoActionType
    {
        ScanTextFromARCamera,
        ScanTextFromImage,
        ScanTextFromLiveCamera,
        ResourcePage,
        Close
    }

    public class TextRecognizerDemoAction : DemoActionBehaviour<TextRecognizerDemoActionType>
    { }

    //public class TextRecognizerDemoAction
    //{
 //   public static TextRecognizerDemoActionType DemoActionType;
    //}
}