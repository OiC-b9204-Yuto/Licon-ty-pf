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

    //ライフ関連
    private Image[] hpHeartContents;
    public int hpHeartQuantity { get; private set; }
    private const int HPHEART_MAX = 5;

    //リーフ関連
    private Image[] leafIcon;
    public int leafQuantity { get; private set; }
    private const int LEAF_MAX = 5;

    //コマンド表示関連
    [SerializeField] Image growUpIcon;
    [SerializeField] Sprite[] growUpIconSprites = new Sprite[2];
    [SerializeField] Image degenerateIcon;
    [SerializeField] Sprite[] degenerateIconSprites = new Sprite[2];

    public void PlayAnimation(string animname) { animator.SetTrigger(animname); }

    //スキル関連
    private GameObject skillIcon;
    private Image skillCoolTimeImage;
    private bool coolTimeStart = false;

    //風のスタミナ関連
    private CanvasGroup windStamina;
    private Image windStaminaGauge;
    //フェード速度の定数
    private const float windStaminaFadeSpeed = 4.0f;
    //フェード開始フラグ trueでフェードイン falseでフェードアウト
    private bool windStaminaFadeStart;
    //非表示になるまでの持続時間の定数とタイマー変数
    private const float windStaminaViewDurationTime = 2.0f;
    private float windStaminaViewDurationTimer;



    public void Initialize()
    {
        //プレイヤースクリプトの取得
        pStatus = player.GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        playerAttack = player.GetComponent<PlayerAttack>();

        //HPオブジェクトの取得
        GameObject Parent_HPHeart = GameObject.Find("HPHearts");
        hpHeartContents = new Image[HPHEART_MAX];
        for (int i = 0; i < HPHEART_MAX; i++)
        {
            hpHeartContents[i] = Parent_HPHeart.transform.GetChild(i + 1).GetComponent<Image>();
        }
        hpHeartQuantity = pStatus.HP;

        //リーフオブジェクトの取得
        GameObject Parent_Leaf = GameObject.Find("Leaves");
        leafIcon = new Image[LEAF_MAX];
        for (int i = 0; i < LEAF_MAX; i++)
        {
            leafIcon[i] = Parent_Leaf.transform.GetChild(i + 1).GetComponent<Image>();
        }
        leafQuantity = pStatus.Leaf;

        degenerateIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);
        growUpIcon.color = new Color(1.0f, 1.0f, 1.0f, 0);

        //スキルアイコンの取得
        skillIcon = GameObject.Find("SkillIcon");
        skillCoolTimeImage = skillIcon.transform.GetChild(0).GetComponent<Image>();
        skillIcon.SetActive(false);

        //スタミナオブジェクトの取得
        windStamina = GameObject.Find("WindStamina").GetComponent<CanvasGroup>();
        windStaminaGauge = windStamina.transform.GetChild(0).GetComponent<Image>();
        windStamina.alpha = 0;

        ReflectLife();
        ReflectLeaf();
    }

    public void UpdateStatus()
    {
        //それまでのHP、リーフ数を取得
        int beforeHP = hpHeartQuantity;
        int beforeLeaf = leafQuantity;

        //新しくステータス値を代入
        hpHeartQuantity = pStatus.HP;
        leafQuantity = pStatus.Leaf;

        //値が変わっていれば反映処理をする
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

        //スキルアイコンの処理
        if (pStatus.isAttack)
        {
            skillIcon.SetActive(true);
            //スキルCTのアニメーション
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

        //スタミナゲージの処理
        WindStaminaUpdate();
    }

    private void WindStaminaUpdate()
    {
        //ゲージ処理
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

        //表示チェック
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

        //フェード処理
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
        //ハート
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
        //リーフ
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
