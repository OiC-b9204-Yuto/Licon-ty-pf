using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Licon.Gimmick;
using System;

public class PlayerStatusUIManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerMove pStatus;
    private Animator animator;
    private PlayerAttack playerAttack;

    //���C�t�֘A
    private Image[] hpHeartContents;
    public int hpHeartQuantity { get; private set; }
    private const int HPHEART_MAX = 5;

    //���[�t�֘A
    private Image[] leafIcon;
    public int leafQuantity { get; private set; }
    private const int LEAF_MAX = 5;

    //�R�}���h�\���֘A
    [SerializeField] Image growUpIcon;
    [SerializeField] Sprite[] growUpIconSprites = new Sprite[2];
    [SerializeField] Image degenerateIcon;
    [SerializeField] Sprite[] degenerateIconSprites = new Sprite[2];

    public void PlayAnimation(string animname) { animator.SetTrigger(animname); }

    //�X�L���֘A
    private GameObject skillIcon;
    private Image skillCoolTimeImage;
    private bool coolTimeStart = false;

    //���̃X�^�~�i�֘A
    private CanvasGroup windStamina;
    private Image windStaminaGauge;
    //�t�F�[�h���x�̒萔
    private const float windStaminaFadeSpeed = 4.0f;
    //�t�F�[�h�J�n�t���O true�Ńt�F�[�h�C�� false�Ńt�F�[�h�A�E�g
    private bool windStaminaFadeStart;
    //��\���ɂȂ�܂ł̎������Ԃ̒萔�ƃ^�C�}�[�ϐ�
    private const float windStaminaViewDurationTime = 2.0f;
    private float windStaminaViewDurationTimer;



    public void Initialize()
    {
        //�v���C���[�X�N���v�g�̎擾
        pStatus = player.GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        playerAttack = player.GetComponent<PlayerAttack>();

        //HP�I�u�W�F�N�g�̎擾
        GameObject Parent_HPHeart = GameObject.Find("HPHearts");
        hpHeartContents = new Image[HPHEART_MAX];
        for (int i = 0; i < HPHEART_MAX; i++)
        {
            hpHeartContents[i] = Parent_HPHeart.transform.GetChild(i + 1).GetComponent<Image>();
        }
        hpHeartQuantity = pStatus.HP;

        //���[�t�I�u�W�F�N�g�̎擾
        GameObject Parent_Leaf = GameObject.Find("Leaves");
        leafIcon = new Image[LEAF_MAX];
        for (int i = 0; i < LEAF_MAX; i++)
        {
            leafIcon[i] = Parent_Leaf.transform.GetChild(i + 1).GetComponent<Image>();
        }
        leafQuantity = pStatus.Leaf;

        degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
        growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);

        //�X�L���A�C�R���̎擾
        skillIcon = GameObject.Find("SkillIcon");
        skillCoolTimeImage = skillIcon.transform.GetChild(0).GetComponent<Image>();
        skillIcon.SetActive(false);

        //�X�^�~�i�I�u�W�F�N�g�̎擾
        windStamina = GameObject.Find("WindStamina").GetComponent<CanvasGroup>();
        windStaminaGauge = windStamina.transform.GetChild(0).GetComponent<Image>();
        windStamina.alpha = 0;

        ReflectLife();
        ReflectLeaf();
    }

    public void UpdateStatus()
    {
        //����܂ł�HP�A���[�t�����擾
        int beforeHP = hpHeartQuantity;
        int beforeLeaf = leafQuantity;

        //�V�����X�e�[�^�X�l����
        hpHeartQuantity = pStatus.HP;
        leafQuantity = pStatus.Leaf;

        //�l���ς���Ă���Δ��f����������
        if(beforeHP != hpHeartQuantity)
        {
            ReflectLife();
        }
        if(beforeLeaf != leafQuantity)
        {
            ReflectLeaf();
        }
        IAction gimmickAC;
        try
        {
            gimmickAC = pStatus.frontGimmick.GetComponent<IAction>();
            if (gimmickAC != null)
            {
                switch (gimmickAC.GetActionType())
                {
                    case ActionType.None:
                        degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
                        growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
                        break;
                    case ActionType.Growth:
                        growUpIcon.sprite = growUpIconSprites[0];
                        growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
                        break;
                    case ActionType.Degenerate:
                        degenerateIcon.sprite = degenerateIconSprites[0];
                        growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
                        degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        break;
                }
            }
            else
            {
                degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
                growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
            }
        }
        catch(Exception e)
        {
            degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
            growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
        }

        //�X�L���A�C�R���̏���
        if (pStatus.isAttack)
        {
            skillIcon.SetActive(true);
            //�X�L��CT�̃A�j���[�V����
            if (coolTimeStart != playerAttack.isCoolDown)
            {
                coolTimeStart = playerAttack.isCoolDown;
                skillCoolTimeImage.fillAmount = coolTimeStart ? 1 : 0;
            }

            if (coolTimeStart)
            {
                skillCoolTimeImage.fillAmount -= Time.deltaTime / PlayerAttack.CoolTime;
            }
        }

        //�X�^�~�i�Q�[�W�̏���
        WindStaminaUpdate();
    }

    private void WindStaminaUpdate()
    {
        //�Q�[�W����
        if (windStamina.alpha > 0)
        {
            float windTimer = pStatus.GetWindTimer();
            if (windTimer > 0)
            {
                windStaminaGauge.fillAmount = 1 - pStatus.windDeltaTimer / windTimer;
            }
            else
            {
                windStaminaGauge.fillAmount = 1;
            }
        }

        //�\���`�F�b�N
        if (pStatus.isWindArea)
        {
            windStaminaFadeStart = true;
            windStaminaViewDurationTimer = windStaminaViewDurationTime;
        }
        else
        {
            if (windStaminaViewDurationTimer > 0)
            {
                windStaminaViewDurationTimer -= Time.deltaTime;
            }
            else
            {
                windStaminaFadeStart = false;
            }
        }

        //�t�F�[�h����
        if (windStaminaFadeStart)
        {
            if (windStamina.alpha < 1) windStamina.alpha += Time.deltaTime * windStaminaFadeSpeed;
        }
        else
        {
            if (windStamina.alpha > 0) windStamina.alpha -= Time.deltaTime * windStaminaFadeSpeed;
        }
    }

    public void ReflectLife()
    {
        //�n�[�g
        for (int i = 0; i < HPHEART_MAX; i++)
        {
            if (i < hpHeartQuantity)
            {
                hpHeartContents[i].gameObject.SetActive(true);
            }
            else
            {
                hpHeartContents[i].gameObject.SetActive(false);
            }
        }
    }

    public void ReflectLeaf()
    {
        //���[�t
        for (int i = 0; i < LEAF_MAX; i++)
        {
            if (i < leafQuantity)
            {
                leafIcon[i].gameObject.SetActive(true);
            }
            else
            {
                leafIcon[i].gameObject.SetActive(false);
            }
        }
    }
}
