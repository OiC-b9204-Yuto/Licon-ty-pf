using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScriptR : MonoBehaviour
{

    public float openSpeedR; //ドアオープンスピード
    public  bool doorOpenR; //ドアチェック
    private float zMovementR; //ドアの移動量
    private float zMoevamountR;//ドアの移動値
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        doorOpenR = false;
        zMovementR = 0.0f;
        zMoevamountR = 1.0f;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;
        // 座標を取得
        Vector3 pos = myTransform.position;

        if (doorOpenR)
        {
            //if (zMovementR <= zMoevamountR)
            //{
            //    zMovementR += openSpeedR;
            //    pos.z -= openSpeedR;    // openSpeedの速度移動
            //    //座標の更新
            //    myTransform.position = pos;
            //}
            // animator.Setbool("Speed",isOpen.true);
        }
    }

}
