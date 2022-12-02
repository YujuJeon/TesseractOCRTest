#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;
using System;
using UnityEngine;

namespace VoxelBusters.EasyMLKit.Editor.Android
{
    public class AndroidGradlePropertiesPostProcessor : IPostGenerateGradleAndroidProject
    {
        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            string targetFilePath = GetGradlePropertiesFilePath(basePath);
            string[] lines = File.ReadAllLines(targetFilePath);

            StringBuilder builder = new StringBuilder();

            foreach(string each in lines)
            {
                string updatedLine = PatchAAPTOptions(each, builder);
                builder.AppendLine(updatedLine);
            }
            File.WriteAllText(targetFilePath, builder.ToString());
        }

        public int callbackOrder { get { return 1; } }

        private string GetGradlePropertiesFilePath(string basePath)
        {
                string gradlePropertiesFileName = "gradle.properties";
                string targetFilePath = Path.Combine(basePath + gradlePropertiesFileName);
                if (File.Exists(targetFilePath))
                {
                    return targetFilePath;
                }

                //Check one level up
                targetFilePath = Path.Combine(basePath, "..", gradlePropertiesFileName);

                if(File.Exists(targetFilePath))
                {
                    return targetFilePath;
                }
                else
                {
                    throw new Exception(string.Format("Failed finding {0}", targetFilePath));
                }
        }

        private string PatchAAPTOptions(string line, StringBuilder builder)
        {
            if (line.StartsWith("unityStreamingAssets"))
            {
                return line + ", " + "tflite";
            }

            return line;
        }
    }
}
#endif
