using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommon;

public class IGPMExitMenuControl : BMenuControl
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
}
