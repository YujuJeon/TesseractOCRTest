using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Toggle introObj;
    public Toggle firstScan;
    public Toggle scanObj;
    public Toggle firistRestock;
    public Toggle restockObj;
    public Toggle secondIntro;
    public TextMeshProUGUI txtResult;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtResult.text = "can docked";


        if (!(firstScan.isOn || scanObj.isOn))
        {
            txtResult.text = "can no docked";
        }
        
            
        
    }
}
