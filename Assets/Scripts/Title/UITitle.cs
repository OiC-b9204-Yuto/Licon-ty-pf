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

        //������
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
            //�V�[���J�n
            case TitleSceneState.TSS_INTROANIM:
                //�C���g���A�j���[�V�����I�����A���j���[�Z���N�g��ʂ�
                if(frontMenuController.CheckMenuInAnimEnd())
                {
                    tState = TitleSceneState.TSS_FRONTMENU;
                }
                break;
            //�t�����g���j���[���쒆
            case TitleSceneState.TSS_FRONTMENU:
                frontMenuController.UpdateMenu();
                break;
            //�I�v�V�������j���[���쒆
            case TitleSceneState.TSS_OPTIONMENU:
                if(optionMenuManager.coroutine != null){ return; }
                //Y�L�[�܂���A�L�[�Ńt�����g���j���[�֖߂�
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
            //�Q�[���I�����j���[���쒆
            case TitleSceneState.TSS_EXITMENU:
                exitMenuController.UpdateMenu();
                break;
            //�V�[���ړ�
            case TitleSceneState.TSS_CHANGESCENE:
                //�t�F�[�h�A�E�g�A�j���[�V�����I�����A�X�e�[�W�Z���N�g��ʂ�
                if (animator.isEndAnimation("Out"))
                {
                    SceneManager.LoadScene("Scene_StageSelect");
                }
                break;
        }
    }


    public void FStartMenu_Press()
    {
        //�t�����g���j���[:�Q�[���X�^�[�g���������Ƃ��̏���
        frontMenuController.DecideGameStartMenu();
        animator.SetTrigger("Out");
        tState = TitleSceneState.TSS_CHANGESCENE;
    }
    public void FOptionMenu_Press()
    {
        //�t�����g���j���[:�I�v�V�������������Ƃ��̏���
        frontMenuController.DeactiveMenu();
        optionMenuManager.OpenMenu();
        tState = TitleSceneState.TSS_OPTIONMENU;
    }
    public void FExitMenu_Press()
    {
        Application.Quit();
        //�t�����g���j���[:�Q�[���I�����������Ƃ��̏���
        /*
        frontMenuController.DeactiveMenu();
        exitMenuController.OpenMenu();
        tState = TitleSceneState.TSS_EXITMENU;
        */

    }

    /*
    public void EGEndMenu_Press()
    {
        //�Q�[���I�����j���[:�Q�[���I�����������Ƃ��̏���
    }
    public void EGTitleBackMenu_Press()
    {
        //�Q�[���I�����j���[����āA�t�����g���j���[�𑀍�\�ɂ���
        frontMenuController.ActiveMenu();
        exitMenuController.CloseMenu();
        tState = TitleSceneState.TSS_FRONTMENU;
    }
    */
}
