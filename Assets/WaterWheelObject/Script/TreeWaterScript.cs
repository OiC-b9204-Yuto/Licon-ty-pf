using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWaterScript : SourceWaterScript
{
    public override void Initialize()
    {
        base.Initialize();
        //初めの速度は0に設定
        WaterSpeed = 0.0f;
        //初めは水はせき止められている
        WaterMove = false;
    }

    public override void UpdateWater()
    {
        //時間が経つに連れて速度増加、速度1.0fでクリッピング
        //速度増加は木の退化速度の倍程度で
        WaterSpeed += 0.4f * Time.deltaTime;
        if (WaterSpeed > 1.0f)
        {
            WaterSpeed = 1.0f;
        }
        base.UpdateWater();
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
