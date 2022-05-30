using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Licon.Gimmick {

    /// <summary>
    /// ギミックの基底クラス
    /// Activeを切り替えることでイベントが呼ばれるので、
    /// イベントにAddListenerで追加してください
    /// </summary>
    public class BaseGimmick : MonoBehaviour
    {
        [SerializeField] protected List<GimickCondition> conditionList = new List<GimickCondition>();
        //ギミックが有効か無効か判断するため（成長状態・退化状態の場合もある）
        [SerializeField] private bool active;
        //アクティブへ変更された時に実行するイベント
        [SerializeField] protected UnityEvent ActivateEvents = new UnityEvent();
        //非アクティブへ変更された時に実行するイベント
        [SerializeField] protected UnityEvent DeactivateEvents = new UnityEvent();

        public Action _endAction;

        public bool Active { 
            get { return active; }
            protected set {
                if (active != value) {
                    if (value)
                    {
                        ActivateEvents.Invoke();
                        active = value;
                    }
                    else
                    {
                        DeactivateEvents.Invoke();
                        active = value;
                    }
                } 
            } 
        }

#if UNITY_EDITOR
        [ContextMenu("Active:true")]
        private void SetActiveIsTrue()
        {
            Active = true;
        }
        [ContextMenu("Active:false")]
        private void SetActiveIsFalse()
        {
            Active = false;
        }
#endif
        /// <summary>
        /// ギミックが動作する条件が満たしているか
        /// ※常に確認する手法のため重い場合は改善します。
        /// </summary>
        /// <returns>true:満たしている場合</returns>
        public bool ConditionCheck()
        {
            foreach (var item in conditionList)
            {
                if (item.baseGimmick && item.baseGimmick.active != item.comp)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// ギミックの動作条件用構造体
    /// </summary>
    [System.Serializable]
    public struct GimickCondition
    {
        public BaseGimmick baseGimmick;
        public bool comp;
    }
}
