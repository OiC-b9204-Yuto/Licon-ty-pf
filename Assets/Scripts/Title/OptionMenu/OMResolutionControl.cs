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
        //�܂��e�O���[�v���A�j���[�V���������s���A���̌�ɌʂɊe���j���[��
        //�A�j���[�V���������s����
        animator.SetTrigger("In");
        animator.Update(0);
        //�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        //���Ԃ̋L�^
        float stime = Time.unscaledTime;
        //���j���[�C���A�j���[�V�����̎��s
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (stime + menuInAnimDelay * i > Time.unscaledTime)
            {
                yield return null;
            }
            entryMenus[i].PlayInAnimation();
        }
        //���j���[�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (!entryMenus[i].animator.isEndAnimation("In"))
            {
                yield return null;
            }
        }
        //��������悤�ɂ���
        ActiveMenu();
        entryMenus[selectMenuNum].Focus();
        coroutine = null;
    }
}
