using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommon;

public class IGPauseMenuManager : BMultiMenuManager
{
    [SerializeField] Text pageText;

    public override void OpenMenu()
    {
        //�|�[�Y���j���[�\������
        SelectMenuNum = 0;
        pageText.text = $"{SelectMenuNum + 1}/{menuControl.Length}";
        coroutine = StartCoroutine(FadeInMenu());
    }

    public override void SelectMenuUpdate()
    {
        int beforeMenuNum = SelectMenuNum;
        base.SelectMenuUpdate();
        if (SelectMenuNum != beforeMenuNum)
        {
            pageText.text = $"{SelectMenuNum + 1}/{menuControl.Length}";
        }
    }
}
