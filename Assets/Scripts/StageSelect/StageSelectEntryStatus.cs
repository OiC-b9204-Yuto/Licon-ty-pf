using UnityEngine;
using UnityEngine.UI;
public class StageSelectEntryStatus : BButtonMenuEntry
{
    [SerializeField] string stageExpText;
    public override void SelectMenu()
    {
        //選択時処理
        if (!isSelect)
        {
            //選択時処理
            //選択用スプライトへ変更
            //image.sprite = imageSelectSprite;
			image.color = imageSelectColor;
            text.text = stageExpText;
            isSelect = true;
        }
    }

    public override void DeselectMenu()
    {
        if (isSelect)
        {
            //非選択時処理
            //非選択用スプライトへ変更
            //image.sprite = imageUnSelectSprite;
			image.color = imageUnSelectColor;
            isSelect = false;
        }
    }

}
