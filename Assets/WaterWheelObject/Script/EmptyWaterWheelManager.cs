using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWaterWheelManager : WaterWheelManager
{
    //�������܂��Ă��邩
    bool FullWater;
    //�����m�I�u�W�F�N�g�̐�
    int Count;
    //�����m�I�u�W�F�N�g�e
    GameObject DetectionWaterParent;
    //�����m�I�u�W�F�N�g
    GameObject[] DetectionWater;
    //�����m�X�N���v�g
    DetectionWaterScript[] DetectionWaterScript;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        //�����m�I�u�W�F�N�g�̓ǂݍ���
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
        //�������܂��Ă���ꍇ�A���ԂƂ��Ă̋@�\�����悤�ɂȂ�
        //�������܂��Ă��Ȃ��ꍇ���A�Ή����鐅�H���S�ĉ������Ă���ꍇ�ɐ��𒙂߂�
        if (FullWater)
        {
            base.Update();
        }
        else if(CheckWater(Count) && !FullWater)
        {
            FillWater();
        }
    }

    //���𒙂߂�֐�
    void FillWater()
    {
        //����Y���W���㏸�����邱�ƂŐ������񂾂񗭂܂��Ă����\��������
        Water.transform.localPosition += new Vector3(0.0f, 0.5f, 0.0f) * Time.deltaTime;
        if(Water.transform.localPosition.y > 0.0f)
        {
            Water.transform.localPosition = new Vector3(0.0f, 0.0f, 4.0f);
            FullWater = true;
        }
    }

    //�Ή����鐅�H���������Ă��邩�ǂ����`�F�b�N����֐�
    //�����Fint num    �����m�I�u�W�F�N�g�̐�
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
