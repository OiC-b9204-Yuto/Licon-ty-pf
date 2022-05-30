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
        //初期化
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
        //初回メニューを開いたときの処理
        coroutine = StartCoroutine(FadeInMenu());
    }

    public virtual void UpdateMenu()
    {
        //インアニメーションが終わっていなければ更新しない
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
        //まず親グループがアニメーションを実行し、その後に個別に各メニューの
        //アニメーションを実行する
        animator.SetTrigger("In");
        animator.Update(0);
        //インアニメーションが終わっていなければ更新しない
        while (!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        //時間の記録
        float stime = Time.unscaledTime;
        //メニューインアニメーションの実行
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (stime + menuInAnimDelay * i > Time.unscaledTime)
            {
                yield return null;
            }
            entryMenus[i].PlayInAnimation();
        }
        //メニューインアニメーションが終わっていなければ更新しない
        for (int i = 0; i < entryMenus.Length; i++)
        {
            while (!entryMenus[i].animator.isEndAnimation("In"))
            {
                yield return null;
            }
        }
        //動かせるようにする
        ActiveMenu();
        entryMenus[selectMenuNum].Focus();
        coroutine = null;
    }

    public virtual IEnumerator FadeOutMenu()
    {
        //時間の記録
        float stime = Time.unscaledTime;
        //メニューインアニメーションの実行
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
        //表示は変えずに操作可能にする
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        entryMenus[selectMenuNum].Focus();
    }

    public void DeactiveMenu()
    {
        //表示は変えずに操作不可能にする
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public bool CheckMenuInAnimEnd()
    {
        //インアニメーション終了時Trueを返す
        if (coroutine != null)
        {
            return false;
        }
        return true;
    }
}
