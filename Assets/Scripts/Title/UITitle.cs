using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UICommon;

public class UITitle : MonoBehaviour
{
    [SerializeField] TTFrontMenuControl frontMenuController;
    [SerializeField] TTOptionMenuManager optionMenuManager;
    [SerializeField] TTExitMenuControl exitMenuController;

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;

    private Animator animator;
    private TitleSceneState tState;

    void Start()
    {
        animator = GetComponent<Animator>();

        //初期化
        frontMenuController.Initialize();
        optionMenuManager.Initialize();
        exitMenuController.Initialize();

        frontMenuController.OpenMenu();
        tState = TitleSceneState.TSS_INTROANIM;

        Data.SetBGMVolume(BGMSource);
    }
    void Update()
    {
        switch(tState)
        {
            //シーン開始
            case TitleSceneState.TSS_INTROANIM:
                //イントロアニメーション終了時、メニューセレクト画面へ
                if(frontMenuController.CheckMenuInAnimEnd())
                {
                    tState = TitleSceneState.TSS_FRONTMENU;
                }
                break;
            //フロントメニュー操作中
            case TitleSceneState.TSS_FRONTMENU:
                frontMenuController.UpdateMenu();
                break;
            //オプションメニュー操作中
            case TitleSceneState.TSS_OPTIONMENU:
                if(optionMenuManager.coroutine != null){ return; }
                //YキーまたはAキーでフロントメニューへ戻る
                if(Input.GetKeyDown(KeyCode.JoystickButton0) ||
                    Input.GetKeyDown(KeyCode.JoystickButton3)||
					Input.GetKeyDown(KeyCode.Escape))
                {
                    SEManager.Instance.Play("Cancel");
                    optionMenuManager.CloseMenu();
                    frontMenuController.ActiveMenu();
                    tState = TitleSceneState.TSS_FRONTMENU;
                    return;
                }
                optionMenuManager.SelectMenuUpdate();
                break;
            //ゲーム終了メニュー操作中
            case TitleSceneState.TSS_EXITMENU:
                exitMenuController.UpdateMenu();
                break;
            //シーン移動
            case TitleSceneState.TSS_CHANGESCENE:
                //フェードアウトアニメーション終了時、ステージセレクト画面へ
                if (animator.isEndAnimation("Out"))
                {
                    SceneManager.LoadScene("Scene_StageSelect");
                }
                break;
        }
    }


    public void FStartMenu_Press()
    {
        //フロントメニュー:ゲームスタートを押したときの処理
        frontMenuController.DecideGameStartMenu();
        animator.SetTrigger("Out");
        tState = TitleSceneState.TSS_CHANGESCENE;
    }
    public void FOptionMenu_Press()
    {
        //フロントメニュー:オプションを押したときの処理
        frontMenuController.DeactiveMenu();
        optionMenuManager.OpenMenu();
        tState = TitleSceneState.TSS_OPTIONMENU;
    }
    public void FExitMenu_Press()
    {
        Application.Quit();
        //フロントメニュー:ゲーム終了を押したときの処理
        /*
        frontMenuController.DeactiveMenu();
        exitMenuController.OpenMenu();
        tState = TitleSceneState.TSS_EXITMENU;
        */

    }

    /*
    public void EGEndMenu_Press()
    {
        //ゲーム終了メニュー:ゲーム終了を押したときの処理
    }
    public void EGTitleBackMenu_Press()
    {
        //ゲーム終了メニューを閉じて、フロントメニューを操作可能にする
        frontMenuController.ActiveMenu();
        exitMenuController.CloseMenu();
        tState = TitleSceneState.TSS_FRONTMENU;
    }
    */
}
