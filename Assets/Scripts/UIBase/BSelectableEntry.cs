using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BSelectableEntry : BMenuEntry
{
    [SerializeField] protected EventSystem eventSystem;
    [SerializeField] protected Color textSelectColor;
    [SerializeField] protected Color textUnSelectColor;
    [SerializeField] protected Text menuContentsText;
    protected Selectable selectable;

    [SerializeField] protected int maxSelectNum;
    [SerializeField] protected string[] menuContents;
    protected int SelectNum;

    protected const float MenuSlideDelayTime = 0.25f;
    protected float resentTime;
    void Update()
    {
        try
        {
            //�I����Ԃ��Ƒ���ł���悤��
            if (gameObject == eventSystem.currentSelectedGameObject)
            {
                if (resentTime + MenuSlideDelayTime <= Time.unscaledTime)
                {
                    if (Input.GetAxis("Horizontal") > 0.20f)
                    {
                        SelectNum = Mathf.Min(SelectNum + 1, maxSelectNum);
                        resentTime = Time.unscaledTime;
                        menuContentsText.text = menuContents[SelectNum];
                    }
                    else if (Input.GetAxis("Horizontal") < -0.20f)
                    {
                        SelectNum = Mathf.Max(SelectNum - 1, 0);
                        resentTime = Time.unscaledTime;
                        menuContentsText.text = menuContents[SelectNum];
                    }
                }
            }
        }
        catch(NullReferenceException e)
        {
            return;
        }
    }

    public override void Initialize()
    {
        selectable = GetComponent<Selectable>();
        resentTime = Time.unscaledTime;
        menuContentsText.text = menuContents[SelectNum];
        base.Initialize();
    }
    public override void Focus() { selectable.Select(); }
    public override void ValueChange(int i1)
    {
        SelectNum = i1;
        menuContentsText.text = menuContents[SelectNum];
    }
    public override int GetValueI()
    {
        return SelectNum;
    }

    public override void SelectMenu()
    {
        //�I��������
        if (!isSelect)
        {
            //�I��������
            //�I��p�X�v���C�g�֕ύX
            text.color = textSelectColor;
            isSelect = true;
        }
    }
    public override void DeselectMenu()
    {
        if (isSelect)
        {
            //��I��������
            //��I��p�X�v���C�g�֕ύX
            text.color = textUnSelectColor;
            isSelect = false;
        }
    }
}
