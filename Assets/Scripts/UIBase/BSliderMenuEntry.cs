using UnityEngine;
using UnityEngine.UI;

public class BSliderMenuEntry : BMenuEntry
{
    [SerializeField] protected Color textSelectColor;
    [SerializeField] protected Color textUnSelectColor;

    protected Slider slider;
    public override void Initialize()
    {
        slider = GetComponent<Slider>();
        base.Initialize();
    }
    public override void Focus() { slider.Select(); }
    public override void ValueChange(float f1)
    {
        slider.value = f1;
    }
    public override float GetValueF()
    {
        return slider.value;
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
