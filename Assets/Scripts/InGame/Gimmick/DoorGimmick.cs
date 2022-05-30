using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    /// <summary>
    /// ドア用のスクリプト
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
                //条件達成でドアが開く　(接近したらにする場合はPhysicsかコライダーつけて判定)
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