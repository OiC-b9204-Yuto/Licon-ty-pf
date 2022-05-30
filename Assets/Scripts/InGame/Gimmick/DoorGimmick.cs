using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    /// <summary>
    /// �h�A�p�̃X�N���v�g
    /// </summary>
    public class DoorGimmick : BaseGimmick
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        void Update()
        {
            if (Active == false)
            {
                //�����B���Ńh�A���J���@(�ڋ߂�����ɂ���ꍇ��Physics���R���C�_�[���Ĕ���)
                if (ConditionCheck())
                {
                    Active = true;
                    DoorOpen();
                }
            }
        }

        void DoorOpen()
        {
            animator.SetBool("isOpen", true);
        }

        void DoorClose()
        {
            animator.SetBool("isOpen", false);
        }
    }
}