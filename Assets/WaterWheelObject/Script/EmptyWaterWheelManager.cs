using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWaterWheelManager : WaterWheelManager
{
    //水が溜まっているか
    bool FullWater;
    //水検知オブジェクトの数
    int Count;
    //水検知オブジェクト親
    GameObject DetectionWaterParent;
    //水検知オブジェクト
    GameObject[] DetectionWater;
    //水検知スクリプト
    DetectionWaterScript[] DetectionWaterScript;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        //水検知オブジェクトの読み込み
        DetectionWaterParent = this.gameObject.transform.GetChild(3).gameObject;
        Count = DetectionWaterParent.transform.childCount;
        DetectionWater = new GameObject[Count];
        DetectionWaterScript = new DetectionWaterScript[Count];
        for(int i = 0; i < Count; i++)
        {
            DetectionWater[i] = DetectionWaterParent.gameObject.transform.GetChild(i).gameObject;
            DetectionWaterScript[i] = DetectionWater[i].GetComponent<DetectionWaterScript>();
        }

        FullWater = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //水が溜まっている場合、水車としての機能を持つようになる
        //水が溜まっていない場合かつ、対応する水路が全て解放されている場合に水を貯める
        if (FullWater)
        {
            base.Update();
        }
        else if(CheckWater(Count) && !FullWater)
        {
            FillWater();
        }
    }

    //水を貯める関数
    void FillWater()
    {
        //水のY座標を上昇させることで水がだんだん溜まっていく表現をする
        Water.transform.localPosition += new Vector3(0.0f, 0.5f, 0.0f) * Time.deltaTime;
        if(Water.transform.localPosition.y > 0.0f)
        {
            Water.transform.localPosition = new Vector3(0.0f, 0.0f, 4.0f);
            FullWater = true;
        }
    }

    //対応する水路が解放されているかどうかチェックする関数
    //引数：int num    水検知オブジェクトの数
    bool CheckWater(int num)
    {
        bool CheckWater = true;
        for (int i = 0; i < num; i++)
        {
            if (!DetectionWaterScript[i].GetDetectionWater())
            {
                CheckWater = false;
                break;
            }
        }
        return CheckWater;
    }
}
