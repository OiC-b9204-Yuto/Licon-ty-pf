using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionWaterScript : MonoBehaviour
{
    bool DetectionWater;

    public void Initialize()
    {
        DetectionWater = false;
    }

    //Getter
    public bool GetDetectionWater()
    {
        return DetectionWater;
    }

    //LCN_ENV_EP_WATERPLANEÇ∆è’ìÀÇµÇΩèÍçáÅADetectionWaterÇTrueÇ…Ç∑ÇÈ
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "LCN_ENV_EP_WATERPLANE")
        {
            DetectionWater = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
