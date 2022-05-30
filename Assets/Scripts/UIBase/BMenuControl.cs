using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UICommon;

public class BMenuControl : MonoBehaviour
{
    [SerializeField] protected EventSystem eSystem;
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    public Coroutine coroutine { get; protected set; }

    [SerializeField] protected float menuInAnimDelay;
    [SerializeField] protected BMenuEntry[] entryMenus;
    protected int selectMenuNum = 0;

    public virtual void Initialize()
    {
        //������
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        coroutine = null;

        for (int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].Initialize();
        }
        selectMenuNum = 0;
    }
    public virtual void OpenMenu()
    {
        //���񃁃j���[���J�����Ƃ��̏���
        coroutine = StartCoroutine(FadeInMenu());
    }

    public virtual void UpdateMenu()
    {
        //�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        if (coroutine != null)
        {
            return;
        }
        for (int i = 0; i < entryMenus.Length; i++)
        {
            if(entryMenus[i].SpriteUpdate(eSystem.currentSelectedGameObject))
            {
                if(selectMenuNum != i)
                {
                    SEManager.Instance.Play("CursorMove");
                }
                selectMenuNum = i;
            }
        }
    }

    public virtual void CloseMenu()
    {
        animator.SetTrigger("Out");
        for (int i = 0; i < entryMenus.Length; i++)
        {
            entryMenus[i].PlayOutAnimation();
        }
        SEManager.Instance.Play("Cancel");
        DeactiveMenu();
    }

    public virtual IEnumerator FadeInMenu()
    {
        //�܂��e�O���[�v���A�j���[�V���������s���A���̌�ɌʂɊe���j���[��
        //�A�j���[�V���������s����
        animator.SetTrigger("In");
        animator.Update(0);
        //�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        //���Ԃ̋L�^
        float stime = Time.unscaledTime;
        //���j���[�C���A�j���[�V�����̎��s
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (stime + menuInAnimDelay * i > Time.unscaledTime)
            {
                yield return null;
            }
            entryMenus[i].PlayInAnimation();
        }
        //���j���[�C���A�j���[�V�������I����Ă��Ȃ���΍X�V���Ȃ�
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (!entryMenus[i].animator.isEndAnimation("In"))
            {
                yield return null;
            }
        }
        //��������悤�ɂ���
        ActiveMenu();
        entryMenus[selectMenuNum].Focus();
        coroutine = null;
    }

    public virtual IEnumerator FadeOutMenu()
    {
        //���Ԃ̋L�^
        float stime = Time.unscaledTime;
        //���j���[�C���A�j���[�V�����̎��s
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (stime + menuInAnimDelay * i > Time.unscaledTime)
            {
                yield return null;
            }
            entryMenus[i].SetSceneExitFlg();
        }
    }

    public void ActiveMenu()
    {
        //�\���͕ς����ɑ���\�ɂ���
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        entryMenus[selectMenuNum].Focus();
    }

    public void DeactiveMenu()
    {
        //�\���͕ς����ɑ���s�\�ɂ���
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public bool CheckMenuInAnimEnd()
    {
        //�C���A�j���[�V�����I����True��Ԃ�
        if (coroutine != null)
        {
            return false;
        }
        return true;
    }
}
