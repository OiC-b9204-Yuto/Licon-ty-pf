using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScriptR : MonoBehaviour
{

    public float openSpeedR; //�h�A�I�[�v���X�s�[�h
    public  bool doorOpenR; //�h�A�`�F�b�N
    private float zMovementR; //�h�A�̈ړ���
    private float zMoevamountR;//�h�A�̈ړ��l
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
        // transform���擾
        Transform myTransform = this.transform;
        // ���W���擾
        Vector3 pos = myTransform.position;

        if (doorOpenR)
        {
            //if (zMovementR <= zMoevamountR)
            //{
            //    zMovementR += openSpeedR;
            //    pos.z -= openSpeedR;    // openSpeed�̑��x�ړ�
            //    //���W�̍X�V
            //    myTransform.position = pos;
            //}
            // animator.Setbool("Speed",isOpen.true);
        }
    }

}
