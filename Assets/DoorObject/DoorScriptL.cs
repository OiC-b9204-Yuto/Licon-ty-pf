using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScriptL : MonoBehaviour
{
    public float openSpeedL; //ドアオープンスピード
    public bool doorOpenL; //ドアチェック
    private float zMovementL; //ドアの移動量
    private float zMoveamountL;//ドアの移動値

    // Start is called before the first frame update
    void Start()
    {

        doorOpenL = false;
        zMovementL = 0.0f;
        zMoveamountL = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;
        // 座標を取得
        Vector3 pos = myTransform.position;

        if (doorOpenL)
        {
            if (zMovementL <= zMoveamountL)
            {
                zMovementL += openSpeedL;
                pos.z -= openSpeedL;    // openSpeedの速度移動
                //座標の更新
                myTransform.position = pos;
            }
        }
    }
}
