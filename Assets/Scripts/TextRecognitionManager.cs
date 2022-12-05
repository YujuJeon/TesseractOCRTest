using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EasyMLKit;
using VoxelBusters.CoreLibrary;
using TMPro;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

public class TextRecognitionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtDisplay;
    [SerializeField] private ConsoleRect consoleRect;

    public void DisplayMessage()
    {
        for (int i = 0; i < consoleRect.strArr.Count; i++)
        {
            txtDisplay.text = consoleRect.strArr[i].ToString();
        }
    }
}
