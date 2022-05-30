using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public class BMultiMenuManager : MonoBehaviour
{
    [SerializeField] protected BMenuControl[] menuControl;                          //���j���[����X�N���v�g
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    public Coroutine coroutine { get; protected set; }
    protected int SelectMenuNum;                                                    //���ݑI�𒆂̃��j���[�ԍ�
    public virtual void Initialize()
    {
        //������
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < menuControl.Length; i++)
        {
            menuControl[i].Initialize();
        }

        //������
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        coroutine = null;
        SelectMenuNum = 0;
    }

    public virtual void OpenMenu()
    {
        //�|�[�Y���j���[�\������
        SelectMenuNum = 0;
        coroutine = StartCoroutine(FadeInMenu());
    }

    public virtual void SelectMenuUpdate()
    {
        if(coroutine != null)
        {
            return;
        }
        for (int i = 0; i < menuControl.Length; i++)
        {
            if (menuControl[i].coroutine != null)
            {
                return;
            }
        }

        int beforeMenuNum = SelectMenuNum;
        if(Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            SelectMenuNum = Mathf.Min(SelectMenuNum + 1, menuControl.Length - 1);
        }
        else if(Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            SelectMenuNum = Mathf.Max(SelectMenuNum - 1, 0);
        }
        if(SelectMenuNum != beforeMenuNum)
        {
            //�I�����j���[�ԍ����ς���Ă�����ύX
            menuControl[beforeMenuNum].CloseMenu();
            menuControl[SelectMenuNum].OpenMenu();
            SEManager.Instance.Play("CursorMove");
            return;
        }
        //���j���[�X�V����
        menuControl[SelectMenuNum].UpdateMenu();
    }

    public virtual void CloseMenu()
    {
        animator.SetTrigger("Out");
        //���݊J���Ă��郁�j���[�����
        menuControl[SelectMenuNum].CloseMenu();
        DeactiveMenu();
        SEManager.Instance.Play("Cancel");
    }

    public void ActiveMenu()
    {
        //�\���͕ς����ɑ���\�ɂ���
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeactiveMenu()
    {
        //�\���͕ς����ɑ���s�\�ɂ���
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual IEnumerator FadeInMenu()
    {
        //�܂��e�O���[�v���A�j���[�V���������s���A���̌�ɌʂɊe���j���[��
        //�A�j���[�V���������s����
        animator.SetTrigger("In");
        animator.Update(0);
        //�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        while (!isEndAnimation("In"))
        {
            yield return null;
        }
        //��������悤�ɂ���
        ActiveMenu();
        yield return StartCoroutine(menuControl[SelectMenuNum].FadeInMenu());
        coroutine = null;
    }

    public bool isEndAnimation(string animname)
    {
        //����̃A�j���[�V�������I�����Ă��邩
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animname) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
        {
            return true;
        }
        return false;
    }
}
