using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TTFrontMenuControl : BMenuControl
{
    public void DecideGameStartMenu()
    {
        //�V�[���ړ��A�j���[�V���������s
        for(int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].SetSceneExitFlg();
        }
    }
}
