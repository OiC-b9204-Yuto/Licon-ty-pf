using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScriptL : MonoBehaviour
{
    public float openSpeedL; //�h�A�I�[�v���X�s�[�h
    public bool doorOpenL; //�h�A�`�F�b�N
    private float zMovementL; //�h�A�̈ړ���
    private float zMoveamountL;//�h�A�̈ړ��l

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
        // transform���擾
        Transform myTransform = this.transform;
        // ���W���擾
        Vector3 pos = myTransform.position;

        if (doorOpenL)
        {
            if (zMovementL <= zMoveamountL)
            {
                zMovementL += openSpeedL;
                pos.z -= openSpeedL;    // openSpeed�̑��x�ړ�
                //���W�̍X�V
                myTransform.position = pos;
            }
        }
    }
}
