using System.Collections;
using UICommon;
using UnityEngine;

public class OMSoundMenuControl : BMenuControl
{
    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] AudioSource SEAudioSource;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject[] cursorTargetObject;

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void CloseMenu()
    {
        Data.SaveAllData();
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
        //設定済みの音量を適用
        entryMenus[0].ValueChange(Data.gData.BGMVolume);
        entryMenus[1].ValueChange(Data.gData.SEVolume);
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

    public void BGMVolumeChange()
    {
        BGMAudioSource.volume = entryMenus[0].GetValueF();
        Data.SaveBGMVolume(entryMenus[0].GetValueF());
    }
    public void SEVolumeChange()
    {
        SEAudioSource.volume = entryMenus[1].GetValueF();
        Data.SaveSEVolume(entryMenus[1].GetValueF());
    }
}
