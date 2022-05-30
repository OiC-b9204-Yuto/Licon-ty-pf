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
        //�ݒ�ς݂̉��ʂ�K�p
        entryMenus[0].ValueChange(Data.gData.BGMVolume);
        entryMenus[1].ValueChange(Data.gData.SEVolume);
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
