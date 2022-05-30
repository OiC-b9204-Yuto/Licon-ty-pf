using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UICommon;

public class UIStageSelect : MonoBehaviour
{
    [SerializeField] SSStageMenuControl smManager;                    //ステージメニュー制御
    [SerializeField] Image stageView;
    [SerializeField] Sprite[] stageImageSprite;

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;

    private Animator animator;
    private int selectStageNo;                                      //選択されたステージ番号
    private StageSelectUIStatus sceneStatus;                        //現在のUIの状態
    private Coroutine animCoroutine = null;

    void Start()
    {
        //制御スクリプトの初期化
        animator = GetComponent<Animator>();
        smManager.Initialize();

        //変数の初期化
        selectStageNo = 0;
        animCoroutine = StartCoroutine(PlayInAnim());
        sceneStatus = StageSelectUIStatus.SSST_SCENESTART;

        Data.SetBGMVolume(BGMSource);
    }

    void Update()
    {
        switch (sceneStatus)
        {
            case StageSelectUIStatus.SSST_SCENESTART:
                //インアニメーション終了時、選択可能状態へと移る
                if (animCoroutine == null)
                {
                    sceneStatus = StageSelectUIStatus.SSST_INSELECT;
                }
                break;
            case StageSelectUIStatus.SSST_INSELECT:
                //Aボタンでタイトルへ戻る
                if(Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    SEManager.Instance.Play("Cancel");
                    sceneStatus = StageSelectUIStatus.SSST_BACKTITLE;
                    return;
                }
                //ステージ移動操作反映
                smManager.UpdateMenu();
                selectStageNo = smManager.GetSelectStageNum();
                stageView.sprite = stageImageSprite[selectStageNo];
                break;

            case StageSelectUIStatus.SSST_DECIDESTAGE:
                if(animCoroutine == null)
                {
                    SceneManager.LoadScene($"Scene_Stage{Data.currentStageNo}");
                }
                break;
            case StageSelectUIStatus.SSST_BACKTITLE:
                if (animCoroutine == null)
                {
                    SceneManager.LoadScene("Scene_Title");
                }
                break;
        }
    }


    public void PressMenu()
    {
        //ステージ番号を格納(移動シーン先に使用)
        selectStageNo = smManager.GetSelectStageNum();
        if(selectStageNo != -1)
        {
            Data.currentStageNo = selectStageNo + 1;
            smManager.PlayEntryOut();
            smManager.DeactiveMenu();
            animCoroutine = StartCoroutine(PlayOutAnim());
            SEManager.Instance.Play("Decide");
            sceneStatus = StageSelectUIStatus.SSST_DECIDESTAGE;
        }

    }

    public IEnumerator PlayInAnim()
    {
        animator.SetTrigger("In");
        animator.Update(0);
        while(!animator.isEndAnimation("In"))
        {
            yield return null;
        }
        yield return StartCoroutine(smManager.FadeInMenu());
        animCoroutine = null;
    }

    public IEnumerator PlayOutAnim()
    {
        yield return StartCoroutine(smManager.FadeOutMenu());
        animator.SetTrigger("Out");
        animator.Update(0);
        while (!animator.isEndAnimation("Out"))
        {
            yield return null;
        }
        animCoroutine = null;
    }
}
