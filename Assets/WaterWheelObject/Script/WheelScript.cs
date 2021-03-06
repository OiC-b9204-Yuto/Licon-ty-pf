using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    //水車の状態、trueなら水車が動作する、falseならつるが絡みついて動作しない
    bool WheelState;
    //水車の回転する速度、何秒で1回転するかで指定
    //水との速度の関係はRotateSpeed/WaterSpeed=2.5になるようにする
    float RotateSpeed;
    //水車の回転方向、-1,0,1のいずれかで指定
    int RotateDirection;

    //初期化
    public void Initialize()
    {
        WheelState = false;
        RotateSpeed = 5.0f;
        RotateDirection = 1;
    }

    //水車の回転
    public void UpdateWheel()
    {
        //水車の回転
        this.transform.Rotate(360.0f * RotateDirection / RotateSpeed * Time.deltaTime, 0f, 0f);
    }

    //Getter
    public bool GetWheelState()
    {
        return WheelState;
    }

    //Setter
    public void SetWheelState()
    {
        WheelState = true;
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
