using UnityEngine;
using UnityEngine.UI;
public class StageSelectEntryStatus : BButtonMenuEntry
{
    [SerializeField] string stageExpText;
    public override void SelectMenu()
    {
        //�I��������
        if (!isSelect)
        {
            //�I��������
            //�I��p�X�v���C�g�֕ύX
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
            //��I��������
            //��I��p�X�v���C�g�֕ύX
            //image.sprite = imageUnSelectSprite;
			image.color = imageUnSelectColor;
            isSelect = false;
        }
    }

}
