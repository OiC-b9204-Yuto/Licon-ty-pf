using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public class OMResolutionControl : BMenuControl
{
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject[] cursorTargetObject;
    public override void CloseMenu()
    {
        Data.SaveIsFullScreen(entryMenus[0].GetValueI() == 1);
        Data.SaveResolution(entryMenus[1].GetValueI());
        Data.SaveQuality(entryMenus[2].GetValueI());
        Data.SaveAllData();

        Data.SetResolution();
        Data.SetQuality();
        animator.SetTrigger("Out");
        for (int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].PlayOutAnimation();
        }
        DeactiveMenu();
    }

    public override void UpdateMenu()
    {
        int beforeMenuNum = selectMenuNum;
        base.UpdateMenu();
        if (beforeMenuNum != selectMenuNum)
        {
            cursor.GetComponent<RectTransform>().sizeDelta
                = cursorTargetObject[selectMenuNum].GetComponent<RectTransform>().sizeDelta;
            cursor.GetComponent<RectTransform>().position
                = cursorTargetObject[selectMenuNum].GetComponent<RectTransform>().position;
        }
    }



    public override IEnumerator FadeInMenu()
    {
        entryMenus[0].ValueChange(Data.gData.isFullScreen ? 1 : 0);
        entryMenus[1].ValueChange(Data.gData.resolutionNo);
        entryMenus[2].ValueChange(Data.gData.quality);
        //まず親グループがアニメーションを実行し、その後に個別に各メニューの
        //アニメーションを実行する
        animator.SetTrigger("In");
        animator.Update(0);
        //インアニメーションが終わっていなければ更新しない
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        //時間の記録
        float stime = Time.unscaledTime;
        //メニューインアニメーションの実行
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (stime + menuInAnimDelay * i > Time.unscaledTime)
            {
                yield return null;
            }
            entryMenus[i].PlayInAnimation();
        }
        //メニューインアニメーションが終わっていなければ更新しない
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (!entryMenus[i].animator.isEndAnimation("In"))
            {
                yield return null;
            }
        }
        //動かせるようにする
        ActiveMenu();
        entryMenus[selectMenuNum].Focus();
        coroutine = null;
    }
}
