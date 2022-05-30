using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TTFrontMenuControl : BMenuControl
{
    public void DecideGameStartMenu()
    {
        //シーン移動アニメーションを実行
        for(int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].SetSceneExitFlg();
        }
    }
}
