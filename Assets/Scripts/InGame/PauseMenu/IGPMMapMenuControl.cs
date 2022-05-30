using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public class IGPMMapMenuControl : BMenuControl
{
    public override void CloseMenu()
    {
        animator.SetTrigger("Out");
        for (int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].PlayOutAnimation();
        }
        DeactiveMenu();
    }
    public override IEnumerator FadeInMenu()
    {
        animator.SetTrigger("In");
        animator.Update(0);
        //インアニメーションが終わっていなければ更新しない
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        coroutine = null;
        Debug.Log("終了");
    }
}
