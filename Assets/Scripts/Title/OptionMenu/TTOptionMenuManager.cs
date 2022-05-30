using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTOptionMenuManager : BMultiMenuManager
{
	[SerializeField] Sprite[] menuBackSprite = new Sprite[2];
    [SerializeField] Image menuBackImage;
    public override void OpenMenu()
    {
        //ポーズメニュー表示処理
        SelectMenuNum = 0;
        menuBackImage.sprite = menuBackSprite[0];
        coroutine = StartCoroutine(FadeInMenu());
    }
    public override void SelectMenuUpdate()
    {
        int beforeMenuNum = SelectMenuNum;
        base.SelectMenuUpdate();
        if (SelectMenuNum != beforeMenuNum)
        {
            menuBackImage.sprite = menuBackSprite[SelectMenuNum];
        }
    }
}
