using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Licon.Gimmick
{

    public class DoorOpenScript : BaseGimmick
    {
        private Animator animator;
        public AudioClip DoorSE;
        AudioSource audiosource;
        private FX_DOORScript FX_DOORScript;
        private bool IsOpen;                //�h�A���J����p�̕ϐ�
        private bool StartTimer;            //�^�C�}�[���N�������邽�߂̃t���O
        private float DoorTime;             //�M�~�b�N�����̕b���Ǘ��p
        private float DoorTimer;            //�h�A���J���܂ł̎��Ԃ̐ݒ�p
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audiosource = GetComponent<AudioSource>();
            Transform fxdoor = transform.Find("FX_DOOR");
            if(fxdoor)
                FX_DOORScript = fxdoor.GetComponent<FX_DOORScript>();
            IsOpen = false;
            StartTimer = false;
            DoorTimer = 2.0f;
            DoorTime = 0.0f;
        }
        private void Update()
        {
            if(IsOpen)
            {
                if (Active == false)
                {
                audiosource.PlayOneShot(DoorSE);
                animator.SetBool("isOpen", true);
                Active = true;
                if(FX_DOORScript)
                    FX_DOORScript.FX_DOOR = true;
                }
            }
            if(StartTimer)
            {
                DoorTime += Time.deltaTime;
                if(DoorTime>= DoorTimer)
                {
                    IsOpen = true;
                }
            }
            if (ConditionCheck())
            {
                StartTimer = true;
            }
        }
    }
}