using UICommon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Licon.Gimmick;

public class UIInGame : MonoBehaviour
{
    private Animator animator;

    [SerializeField] GoalArea goal;
    [SerializeField] PlayerStatusUIManager pStatusManager;                      //�X�e�[�^�XUI����
    [SerializeField] IGPauseMenuManager pMenuManager;                           //�|�[�Y���j���[����
    [SerializeField] HelpMenuControl helpMenuController;
    [SerializeField] GameClearMenuControl gcMenuController;                     //�Q�[���N���A���j���[����
    [SerializeField] GameOverMenuControl goMenuController;                      //�Q�[���I�[�o�[���j���[����

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;

    [SerializeField] AudioClip gameOverBGM;
    [SerializeField] AudioClip gameClearBGM;

    private string nextSceneName;
    public InGameStatus sceneStatus { get; private set; }                       //�V�[�����
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
        //�eUI�O���[�v�̏�����
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
        //�V�[����Ԃɉ������X�V����
        SceneStatusUpdate();
    }

    private void SceneStatusUpdate()
    {
        //�V�[���󋵂ɂ���ď�����ύX����
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
                //�|�[�Y���j���[���J��(Y�L�[)
                if (Input.GetKeyDown(KeyCode.JoystickButton3) ||
					Input.GetKeyDown(KeyCode.Escape))
                {
                    pMenuManager.OpenMenu();
                    sceneStatus = InGameStatus.IGS_PAUSE;
					//Time.timeScale =0f;
                    return;
                }
                //�w���v���j���[���J��(X�L�[)
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                {
                    helpMenuController.OpenMenu();
                    sceneStatus = InGameStatus.IGS_HELP;
                    return;
                }
                //�Q�[��UI�̍X�V����
                pStatusManager.UpdateStatus();

                //�S�[���G���A�����ŃQ�[���N���A����������
                if(goal.isGoal)
                {
                    gcMenuController.OpenMenu();
                    BGMSource.clip = gameClearBGM;
                    BGMSource.Play();
                    sceneStatus = InGameStatus.IGS_GAMECLEAR;
                }
                //HP0�ŃQ�[���I�[�o�[����
                else if (pStatusManager.hpHeartQuantity <= 0)
                {
                    goMenuController.OpenMenu();
                    BGMSource.clip = gameOverBGM;
                    BGMSource.Play();
                    sceneStatus = InGameStatus.IGS_GAMEOVER;
                }
                
                break;

            case InGameStatus.IGS_PAUSE:
                //�|�[�Y���j���[�����(Y�L�[�AA�L�[)
                if (Input.GetKeyDown(KeyCode.JoystickButton0) ||
                    Input.GetKeyDown(KeyCode.JoystickButton3) ||
					Input.GetKeyDown(KeyCode.Escape))
                {
                    pMenuManager.CloseMenu();
                    sceneStatus = InGameStatus.IGS_INGAME;
                    return;
                }
                //���݂̃|�[�Y���j���[�̍X�V����
                //�|�[�Y���j���[�őI�����ꂽ�V�[���X�e�[�^�X���L�^����
                pMenuManager.SelectMenuUpdate();
                break;

            case InGameStatus.IGS_HELP:
                //�|�[�Y���j���[�����(Y�L�[�AA�L�[)
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
                //�Q�[���I�[�o�[���j���[����
                goMenuController.UpdateMenu();
                break;
            case InGameStatus.IGS_GAMECLEAR:
                //�Q�[���N���A���j���[����
                gcMenuController.UpdateMenu();
                break;
            case InGameStatus.IGS_SCENECHANGE:
                //�A�j���[�V�������I������^�C�g���V�[���ֈړ�
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
        //�|�[�Y���j���[�u�Q�[���֖߂�v���j���[�I��������
        pMenuManager.CloseMenu();
        SEManager.Instance.Play("Cancel");
        sceneStatus = InGameStatus.IGS_INGAME;
    }
    public void PressButtonToTitle()
    {
        //�u�^�C�g���֖߂�v���j���[�I��������
        animator.SetTrigger("Out");
        nextSceneName = "Scene_Title";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonToStageSelect()
    {
        //�u�X�e�[�W�I���֖߂�v���j���[�I��������
        animator.SetTrigger("Out");
        nextSceneName = "Scene_StageSelect";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonToNextStage()
    {
        //�u���̃X�e�[�W�ցv���j���[�I��������
        animator.SetTrigger("Out");
        nextSceneName = $"Scene_Stage{Data.GetNextStageNo()}";
        Data.currentStageNo = Data.GetNextStageNo();
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
    public void PressButtonRetry()
    {
        //�u���g���C�v���j���[�I��������
        animator.SetTrigger("Out");
        nextSceneName = $"Scene_Stage{Data.currentStageNo}";
        SEManager.Instance.Play("Decide");
        sceneStatus = InGameStatus.IGS_SCENECHANGE;
    }
}
