using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Licon.Gimmick
{

    public class CraneGmick : BaseGimmick ,IAction
    {
        private Animator animator;
        [SerializeField] private List<WindScript> WindScript;
        private Action EndAction;
        AudioSource audiosource;
        public void Action(Action endAction)
        {
            if (ConditionCheck())
            {
                animator.SetBool("isGrown", true);
                Active = true;
                foreach (var item in WindScript)
                {
                    audiosource.Play();
                    item.DestroyFlg = true;
                }
                EndAction = endAction;
                StartCoroutine(EndAnimation());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            audiosource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            
            GimickCondition condition = new GimickCondition();
            condition.baseGimmick = this;
            condition.comp = Active;
            conditionList.Add(condition);
        }
        private IEnumerator EndAnimation()
        {
            //アニメーション終了待ち
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);
            EndAction?.Invoke();
        }

        public ActionType GetActionType()
        {
            return ConditionCheck() ? ActionType.Growth : ActionType.None;
        }
    }
}
