using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommon;

public class HelpMenuControl : BMenuControl
{
    [SerializeField] Image helpImage;
    [SerializeField] Sprite[] helpImageSprites;
    [SerializeField] GameObject pageIconsObject;
    private Image[] pageIcon;

    private readonly Color selectIconColor = new Color32(255, 255, 255, 255);
    private readonly Color unselectIconColor = new Color32(128, 128, 128, 255);
    public override void Initialize()
    {
        //������
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        coroutine = null;
        selectMenuNum = 0;

        pageIcon = new Image[helpImageSprites.Length];
        for(int i = 0; i < helpImageSprites.Length; i++)
        {
            pageIcon[i] = pageIconsObject.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        helpImage.sprite = helpImageSprites[selectMenuNum];
        for (int i = 0; i < helpImageSprites.Length; i++)
        {
            Color color = selectMenuNum == i ? selectIconColor : unselectIconColor;
            pageIcon[i].color = color;
        }

    }
    public override void OpenMenu()
    {
        //���񃁃j���[���J�����Ƃ��̏���
        coroutine = StartCoroutine(FadeInMenu());
    }

    public override void UpdateMenu()
    {
        if (coroutine != null)
        {
            return;
        }

        int beforeMenuNum = selectMenuNum;
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            selectMenuNum = Mathf.Min(selectMenuNum + 1, helpImageSprites.Length - 1);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            selectMenuNum = Mathf.Max(selectMenuNum - 1, 0);
        }
        if (selectMenuNum != beforeMenuNum)
        {
            //�I�����j���[�ԍ����ς���Ă�����ύX
            helpImage.sprite = helpImageSprites[selectMenuNum];
            for (int i = 0; i < helpImageSprites.Length; i++)
            {
                pageIcon[i].color = selectMenuNum == i ? selectIconColor : unselectIconColor;
            }
            SEManager.Instance.Play("CursorMove");
            return;
        }
    }

    public override void CloseMenu()
    {
        animator.SetTrigger("Out");
        SEManager.Instance.Play("Cancel");
        DeactiveMenu();
    }

    public override IEnumerator FadeInMenu()
    {
        //�A�E�g�A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        /*
        while (!animator.isEndAnimation("Out") || !animator.isEndAnimation("Wait"))
        {
            Debug.Log("BBB");
            yield return null;
        }
        */
        //�A�j���[�V���������s����
        animator.SetTrigger("In");
        animator.Update(0);
        //�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        //��������悤�ɂ���
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        coroutine = null;
    }
}
