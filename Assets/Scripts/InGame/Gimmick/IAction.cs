using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    /// <summary>
    /// アクションを可能にするインターフェース
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// アクションを起こす(成長させる・退化させるなど)
        /// ※ギミックのアニメーション終了時にendActionを実行すること！
        /// </summary>
        void Action(Action endAction);

        /// <summary>
        /// アクションのタイプを返す
        /// ※ 実行できない場合はNoneを返す
        /// </summary>
        ActionType GetActionType();
    }

    public enum ActionType
    {
        None,
        Growth,
        Degenerate
    }

}
