using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UICommon;

public class BMenuToggle : BMenuEntry
{
    [SerializeField] protected EventSystem eventSystem;
    [SerializeField] protected Color textSelectColor;
    [SerializeField] protected Color textUnSelectColor;

    protected Toggle toggle;
    public override bool GetValueB()
    {
        return toggle.isOn;
    }

    public override void Initialize()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = Data.gData.isFullScreen;
        base.Initialize();
    }

    public override void SelectMenu()
    {
        //選択時処理
        if (!isSelect)
        {
            //選択時処理
            //選択用スプライトへ変更
            text.color = textSelectColor;
            isSelect = true;
        }
    }
    public override void DeselectMenu()
    {
        if (isSelect)
        {
            //非選択時処理
            //非選択用スプライトへ変更
            text.color = textUnSelectColor;
            isSelect = false;
        }
    }
}
