using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    /// <summary>
    /// 成長・退化ができる木のギミック
    /// </summary>
    public class TreeGimmick : BaseGimmick ,IAction
    {
        [SerializeField] private Collider collider;
        [SerializeField] private Animator animator;

        [SerializeField] private bool loop = false;


        private void Awake()
        {
            animator.SetBool("isGrow", Active);
            animator.SetBool("isDown", false);
            animator.SetBool("isLoop", loop);
        }

        void Start()
        {
            if (!loop)
            {
                GimickCondition condition = new GimickCondition();
                condition.baseGimmick = this;
                condition.comp = Active;
                conditionList.Add(condition);
            }
            if (Active)
                animator.Play("IDLE");
        }

        //外部からの操作用(プレイヤーから呼ばれるなど)
        public void Action(Action endAction)
        {
            //条件が満たされていない場合は実行しない
            if (!ConditionCheck())
            {
                return;
            }
            _endAction = endAction;
            if (Active)
            {
                StartCoroutine(Degenerate());
            }
            else
            {
                StartCoroutine(Grow());
            }
        }


        /// <summary>
        /// 成長させるための関数
        /// </summary>
        private IEnumerator Grow()
        {
            animator.SetBool("isDown", false);
            animator.SetBool("isGrow", true);
            Active = true;
            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Default"));
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);
            if (collider)
            {
                collider.enabled = true;
            }
            _endAction?.Invoke();
        }

        /// <summary>
        /// 退化させるための関数
        /// </summary>
        private IEnumerator Degenerate()
        {
            animator.SetBool("isGrow", false);
            animator.SetBool("isDown", true);
            Active = false;
            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"));
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);
            if (collider)
            {
                collider.enabled = false;
            }
            _endAction?.Invoke();
        }

        public ActionType GetActionType()
        {
            if (ConditionCheck())
            {
                return Active ? ActionType.Degenerate : ActionType.Growth;
            }
            else
            {
                return ActionType.None;
            }
        }
    }
}