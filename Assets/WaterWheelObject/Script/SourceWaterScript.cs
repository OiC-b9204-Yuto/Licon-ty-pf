using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceWaterScript : MonoBehaviour
{
    //水の長さ
    protected float WaterSize;
    //水の速さ
    protected float WaterSpeed;
    //水が動いているか、trueなら動いている、falseなら止まっている
    protected bool WaterMove;

    //初期化
    public virtual void Initialize()
    {
        WaterSize = 1.5f;
        WaterSpeed = 0.5f;
        WaterMove = true;
    }

    //流れる水
    public virtual void UpdateWater()
    {
        WaterSize += WaterSpeed * Time.deltaTime;
        this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, WaterSize);
    }

    //WaterStopオブジェクトと衝突した場合、水を大きくするのを止める
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WaterStop")
        {
            WaterMove = false;
        }
    }

    //Getter
    public bool GetWaterMove()
    {
        return WaterMove;
    }

    //Setter
    public void SetWaterMove()
    {
        WaterMove = true;
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
