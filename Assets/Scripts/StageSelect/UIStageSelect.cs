using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UICommon;

public class UIStageSelect : MonoBehaviour
{
    [SerializeField] SSStageMenuControl smManager;                    //�X�e�[�W���j���[����
    [SerializeField] Image stageView;
    [SerializeField] Sprite[] stageImageSprite;

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;

    private Animator animator;
    private int selectStageNo;                                      //�I�����ꂽ�X�e�[�W�ԍ�
    private StageSelectUIStatus sceneStatus;                        //���݂�UI�̏��
    private Coroutine animCoroutine = null;

    void Start()
    {
        //����X�N���v�g�̏�����
        animator = GetComponent<Animator>();
        smManager.Initialize();

        //�ϐ��̏�����
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
                //�C���A�j���[�V�����I�����A�I���\��Ԃւƈڂ�
                if (animCoroutine == null)
                {
                    sceneStatus = StageSelectUIStatus.SSST_INSELECT;
                }
                break;
            case StageSelectUIStatus.SSST_INSELECT:
                //A�{�^���Ń^�C�g���֖߂�
                if(Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    SEManager.Instance.Play("Cancel");
                    sceneStatus = StageSelectUIStatus.SSST_BACKTITLE;
                    return;
                }
                //�X�e�[�W�ړ����씽�f
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
        //�X�e�[�W�ԍ����i�[(�ړ��V�[����Ɏg�p)
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
