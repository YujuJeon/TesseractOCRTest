using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EasyMLKit.Internal;

namespace VoxelBusters.EasyMLKit
{
    [Serializable]
    public class FaceDetectorUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors

        public FaceDetectorUnitySettings( bool enabled = true) : base(enabled)
        {
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kFaceDetector;
        }

        #endregion
    }
}