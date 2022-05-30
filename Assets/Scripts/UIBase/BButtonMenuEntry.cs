using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BButtonMenuEntry : BMenuEntry
{
    [SerializeField] protected Sprite imageSelectSprite;
	[SerializeField] protected Color imageSelectColor;
    [SerializeField] protected Color textSelectColor;
    [SerializeField] protected Sprite imageUnSelectSprite;
	[SerializeField] protected Color imageUnSelectColor;
    [SerializeField] protected Color textUnSelectColor;

    protected Image image;
    protected Button button;
    public override void Initialize()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        image.sprite = imageUnSelectSprite;
        base.Initialize();
    }
    public override void Focus() { button.Select(); }

    public override void SelectMenu()
    {
        //選択時処理
        if (!isSelect)
        {
            //選択時処理
            //選択用スプライトへ変更
            image.sprite = imageSelectSprite;
			image.color = imageSelectColor;
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
            image.sprite = imageUnSelectSprite;
			image.color = imageUnSelectColor;
            text.color = textUnSelectColor;
            isSelect = false;
        }
    }
}
