using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWaterScript : SourceWaterScript
{
    public override void Initialize()
    {
        base.Initialize();
        //���߂̑��x��0�ɐݒ�
        WaterSpeed = 0.0f;
        //���߂͐��͂����~�߂��Ă���
        WaterMove = false;
    }

    public override void UpdateWater()
    {
        //���Ԃ��o�ɘA��đ��x�����A���x1.0f�ŃN���b�s���O
        //���x�����͖؂̑މ����x�̔{���x��
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
