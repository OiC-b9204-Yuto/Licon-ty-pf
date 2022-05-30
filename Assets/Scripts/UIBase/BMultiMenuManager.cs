using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public class BMultiMenuManager : MonoBehaviour
{
    [SerializeField] protected BMenuControl[] menuControl;                          //メニュー制御スクリプト
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    public Coroutine coroutine { get; protected set; }
    protected int SelectMenuNum;                                                    //現在選択中のメニュー番号
    public virtual void Initialize()
    {
        //初期化
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < menuControl.Length; i++)
        {
            menuControl[i].Initialize();
        }

        //無効化
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        coroutine = null;
        SelectMenuNum = 0;
    }

    public virtual void OpenMenu()
    {
        //ポーズメニュー表示処理
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
            //選択メニュー番号が変わっていたら変更
            menuControl[beforeMenuNum].CloseMenu();
            menuControl[SelectMenuNum].OpenMenu();
            SEManager.Instance.Play("CursorMove");
            return;
        }
        //メニュー更新処理
        menuControl[SelectMenuNum].UpdateMenu();
    }

    public virtual void CloseMenu()
    {
        animator.SetTrigger("Out");
        //現在開いているメニューを閉じる
        menuControl[SelectMenuNum].CloseMenu();
        DeactiveMenu();
        SEManager.Instance.Play("Cancel");
    }

    public void ActiveMenu()
    {
        //表示は変えずに操作可能にする
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeactiveMenu()
    {
        //表示は変えずに操作不可能にする
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual IEnumerator FadeInMenu()
    {
        //まず親グループがアニメーションを実行し、その後に個別に各メニューの
        //アニメーションを実行する
        animator.SetTrigger("In");
        animator.Update(0);
        //インアニメーションが終わっていなければ更新しない
        while (!isEndAnimation("In"))
        {
            yield return null;
        }
        //動かせるようにする
        ActiveMenu();
        yield return StartCoroutine(menuControl[SelectMenuNum].FadeInMenu());
        coroutine = null;
    }

    public bool isEndAnimation(string animname)
    {
        //特定のアニメーションが終了しているか
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animname) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f)
        {
            return true;
        }
        return false;
    }
}
