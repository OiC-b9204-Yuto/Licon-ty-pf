using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceWaterManager : MonoBehaviour
{
    //êÖÇÃGameObject
    protected GameObject Water;
    //êÖScript
    protected SourceWaterScript WaterScript;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Water = this.gameObject.transform.GetChild(0).gameObject;
        WaterScript = Water.GetComponent<SourceWaterScript>();

        WaterScript.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (WaterScript.GetWaterMove())
        {
            WaterScript.UpdateWater();
        }
    }
}
