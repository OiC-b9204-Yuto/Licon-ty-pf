using UICommon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Licon.Gimmick;

public class UIInGame : MonoBehaviour
{
    private Animator animator;

    [SerializeField] GoalArea goal;
    [SerializeField] PlayerStatusUIManager pStatusManager;                      //ステータスUI制御
    [SerializeField] IGPauseMenuManager pMenuManager;                           //ポーズメニュー制御
    [SerializeField] HelpMenuControl helpMenuController;
    [SerializeField] GameClearMenuControl gcMenuController;                     //ゲームクリアメニュー制御
    [SerializeField] GameOverMenuControl goMenuController;                      //ゲームオーバーメニュー制御

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;

    [SerializeField] AudioClip gameOverBGM;
    [SerializeField] AudioClip gameClearBGM;

    private string nextSceneName;
    public InGameStatus sceneStatus { get; private set; }                       //シーン状態
    public bool CanPlayerControl()
    {
        if(sceneStatus == InGameStatus.IGS_INGAME)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        //各UIグループの初期化
        pStatusManager.Initialize();
        pMenuManager.Initialize();
        helpMenuController.Initialize();
        gcMenuController.Initialize();
        goMenuController.Initialize();

        pStatusManager.PlayAnimation("In");
        sceneStatus = InGameStatus.IGS_INTRO;
        nextSceneName = "";

        Data.SetBGMVolume(BGMSource);
        Data.SetSEVolume(SESource);
    }

    void Update()
    {
        if (CanPlayerControl())
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0;
        }
        //シーン状態に応じた更新処理
        SceneStatusUpdate();
    }

    private void SceneStatusUpdate()
    {
        //シーン状況によって処理を変更する
        switch(sceneStatus)
        {
            case InGameStatus.IGS_INTRO:
                if(animator.isEndAnimation("In"))
                {
                    animator.SetBool("isEndInAnim", true);
                    sceneStatus = InGameStatus.IGS_INGAME;
                }
                break;

            case InGameStatus.IGS_INGAME:
                //ポーズメニューを開く(Yキー)
                if (Input.GetKeyDown(KeyCode.JoystickButton3) ||
					Input.GetKeyDown(KeyCode.Escape))
                {
                    pMenuManager.OpenMenu();
                    sceneStatus = InGameStatus.IGS_PAUSE;
					//Time.timeScale =0f;
                    return;
                }
                //ヘルプメニューを開く(Xキー)
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                {
                    helpMenuController.OpenMenu();
                    sceneStatus = InGameStatus.IGS_HELP;
                    return;
                }
                //ゲームUIの更新処理
                pStatusManager.UpdateStatus();

                //ゴールエリア到着でゲームクリア処理をする
                if(goal.isGoal)
                {
                    gcMenuController.OpenMenu();
                    BGMSource.clip = gameClearBGM;
                    BGMSource.Play();
                    sceneStatus = InGameStatus.IGS_GAMECLEAR;
                }
                //HP0でゲームオーバー処理
                else if (pStatusManager.hpHeartQuantity <= 0)
                {
                    goMenuController.OpenMenu();
                    BGMSource.clip = gameOverBGM;
                    BGMSource.Play();
                    sceneStatus = InGameStatus.IGS_GAMEOVER;
                }
                
                break;

            case InGameStatus.IGS_PAUSE:
                //ポーズメニューを閉じる(Yキー、Aキー)
                if (Input.GetKeyDown(KeyCode.JoystickButton0) ||
                    Input.GetKeyDown(KeyCode.JoystickButton3) ||
					Input.GetKeyDown(KeyCode.Escape))
                {
                    pMenuManager.CloseMenu();
                    sceneStatus = InGameStatus.IGS_INGAME;
                    return;
                }
                //現在のポーズメニューの更新処理
                //ポーズメニューで選択されたシーンステータスを記録する
                pMenuManager.SelectMenuUpdate();
                break;

            case InGameStatus.IGS_HELP:
                //ポーズメニューを閉じる(Yキー、Aキー)
                if (Input.GetKeyDown(KeyCode.JoystickButton0) ||
                    Input.GetKeyDown(KeyCode.JoystickButton2))
                {
                    helpMenuController.CloseMenu();
                    sceneStatus = InGameStatus.IGS_INGAME;
                    return;
                }
                helpMenuController.UpdateMenu();
                break;

            case InGameStatus.IGS_GAMEOVER:
                //ゲームオーバーメニュー処理
                goMenuController.UpdateMenu();
                break;
            case InGameStatus.IGS_GAMECLEAR:
                //ゲームクリアメニュー処理
                gcMenuController.UpdateMenu();
                break;
            case InGameStatus.IGS_SCENECHANGE:
                //アニメーションが終了次第タイトルシーンへ移動
                if (animator.isEndAnimation("Out"))
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(nextSceneName);
                }
                break;
        }
    }

    public void PressButtonPauseMenuClose()
    {
        //ポーズメニュー「ゲームへ戻る」メニュー選択時処理
        pMenuManager.CloseMenu();
        SEManager.Instance.Play("Cancel");
        sceneStatus = InGameStatus.IGS_INGAME;
    }
    public void PressButtonToTitle()
    {
        //「タイトルへ戻る」メニュー選択時処理
        animator.SetTrigger("Out");
        nextSceneName = "Scene_Title";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonToStageSelect()
    {
        //「ステージ選択へ戻る」メニュー選択時処理
        animator.SetTrigger("Out");
        nextSceneName = "Scene_StageSelect";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonToNextStage()
    {
        //「次のステージへ」メニュー選択時処理
        animator.SetTrigger("Out");
        nextSceneName = $"Scene_Stage{Data.GetNextStageNo()}";
        Data.currentStageNo = Data.GetNextStageNo();
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonRetry()
    {
        //「リトライ」メニュー選択時処理
        animator.SetTrigger("Out");
        nextSceneName = $"Scene_Stage{Data.currentStageNo}";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
}
