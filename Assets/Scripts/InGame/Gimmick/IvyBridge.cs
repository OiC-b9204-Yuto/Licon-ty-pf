using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    public enum DireType
    {
        east,
        west,
        south,
        north
    }
    public class IvyBridge : BaseGimmick, IAction
    {
        //モデル差し替えのため変更点が多数あります（竹中）
        [SerializeField] private Collider collider;
        [SerializeField] private Animator animator;

        [SerializeField] private int defaultGrowthState = 0;
        [SerializeField] private int currentGrowthState = 0;
        //各成長状態のデータの配列
        [SerializeField] private float[] GrowLength;
        [SerializeField] private DireType DireTypes;

        //private Collider _collider;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //TestInput();
        }

        private void TestInput()
        {
            //仮入力
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    Action(null);
            //}
        }

        //外部からの操作用(プレイヤーから呼ばれるなど)
        public void Action(Action endAction)
        {
            //条件が満たされていない場合は実行しない
            if (!ConditionCheck())
            {
                return;
            }
            StartCoroutine(Grow());
            _endAction = endAction;
        }

        /// <summary>
        /// 成長させるための関数
        /// </summary>
        private IEnumerator Grow()
        {
            if (currentGrowthState == GrowLength.Length - 1)
            {
                yield break;
            }
            currentGrowthState++;
            Active = true;
            animator.enabled = true;
            //アニメーション終了待ち
            var change = WaitAnimationEnd();
            while (change.MoveNext())
                yield return change.Current;
            collider.enabled = true;
            _endAction?.Invoke();
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (a - b) * t;
        }

        /// <summary>
        /// 成長状態の変更（サイズと座標の変更 テスト用）
        /// </summary>
        private IEnumerator ChangeGrowthState()
        {
            Vector3 beforeScale = transform.localScale;
            var a = new Vector3(beforeScale.x, beforeScale.y, GrowLength[currentGrowthState]);
            for (int i = 0; i < 120; i++)
            {
                transform.localScale = Vector3.Lerp(beforeScale, a, (1.0f / 120.0f) * i);
                yield return null;
            }
        }

        private IEnumerator WaitAnimationEnd()
        {
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
            {
                yield return null;
            }
        }

        public ActionType GetActionType()
        {

            return ConditionCheck() ? ActionType.Growth : ActionType.None;
        }
    }
}
