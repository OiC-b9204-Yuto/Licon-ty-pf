using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Gimmick;
using Licon.Damaged;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] UIInGame ui;
    private CharacterController characterController;    // キャラクターコンポーネント用変数
    private Animator animator;                          //アニメーションコンポーネント用変数

    public float charaSpeed = 5.0f;         // キャラクターのスピード
    public float rotateSpeed = 1200.0f;     // キャラクターの回転スピード
    public float mover = 0.0f;              // キャラクターの入力量
    public float interval = 0.1f;
    [SerializeField]
    private GameObject HitEffect;           //衝突時エフェクト

    //private float hitWindTimer = 0;         //風に当たっている秒数
    //private float blowAwayTimer = 8.0f;     //風に吹き飛ばされるまでの秒数
    public float windDeltaTimer { get; private set; }       //風タイマーのdeltaTime用の変数

    private bool isMoved;                   //移動可否フラグ
    public bool isWindArea { get; private　set; }                //風エリアにいるかのフラグ
    private bool isBlowAway;                // 風に吹き飛ばされているかのフラグ（有効状態の場合は移動できない）
    private bool isSafeArea;                //風エリア内で安全地帯にいるかどうかのフラグ

    private float Resistancevalues;         //抵抗値用
    private string Windname;                //触れている風の名前取得用
    private float blowspeed;                //プレイヤーを吹き飛ばす際の風速用変数（仮置き10.0f）

    public bool isAttack { get; private set; }                  //攻撃可否フラグ

    //キャラクターのHP
    public int HP { get; set; }

    //キャラクターのリーフの数
    public int Leaf { get; private set; }

    //当たり判定
    public bool isCollision { get; private set; }
    
    //点滅中
    public bool isBlinking { get; private set; }

    //ダメージを受けた
    public bool isDamage { get; private set; }

    //アクション中(ギミック発動中)
    public bool isAction { get; set; }

    //引きカメラの範囲内か
    public bool isZoomArea { get; private set; }

    //目の前にあるギミックの参照
    public BaseGimmick frontGimmick { get; private set; }

    //ダメージ処理を行うためのスクリプト変数
    private PlayerDamaged playerDamaged;

    //風用
    private WindScript wind;

    //プレイヤーの攻撃スクリプト変数
    private PlayerAttack playerAttack;

    //初期地点
    private Vector3 StartPos;

    //ゴールエリアスクリプト変数
    private GameObject goalObject;
    private GoalArea goalArea;

    //効果音スクリプト変数
    private SoundEffect soundEffect;

    public float GetWindTimer() => wind ? wind.WindTimer : -1;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerDamaged = GetComponent<PlayerDamaged>();
        playerAttack = GetComponent<PlayerAttack>();
        goalObject = GameObject.Find("GoalArea");
        goalArea = goalObject.GetComponent<GoalArea>();
        soundEffect = GetComponent<SoundEffect>();
        HP = 3;
        Leaf = 3;
        isCollision = false;
        isMoved = true;
        isDamage = false;
        isAction = false;
        isBlowAway = false;
        isSafeArea = false;
        isAttack = false;
        StartPos = transform.position;
        Resistancevalues = 1.0f;
        blowspeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ中はreturn
        if(!ui.CanPlayerControl())
        {
            return;
        }

        if (HP <= 0 || goalArea.isGoal)
        {
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Moving", false);
            isMoved = false;
            return;
        }

        //移動処理
        UpdateMove();

        //アクション処理
        if (isCollision)
        {
            if (frontGimmick.ConditionCheck())
            {
                //ギミックを発動させる
                IAction action = frontGimmick.GetComponent<IAction>();
                if (Input.GetButtonDown("Action") && action != null && Leaf > 0)
                {
                    if(!isAction && frontGimmick.gameObject.tag != "FreeGimmick")
                    {
                        Leaf -= 1;
                    }
                    action.Action(ActionEnd);
                    animator.SetBool("Action", true);
                    isAction = true;
                    isMoved = false;
                }
            }
        }

        //ダメージ処理
        isBlinking = playerDamaged.isDamaged;

        //攻撃処理
        if(Input.GetButton("Aim"))
        {
            //アイテム取得前は攻撃できないようにする
            if (!isAttack)
            {
                return;
            }
            playerAttack.isAiming = true;
            playerAttack.RenderAimingText();
            mover = 0.0f;
            animator.SetFloat("Speed", mover);
            animator.SetBool("Moving", false);
            isMoved = false;
        }
        else
        {
            if(playerAttack.isAiming && !playerAttack.isSkillActive)
            {
                playerAttack.isAiming = false;
                isMoved = true;
            }
        }
    }

    private IEnumerator knockback(Vector3 move)
    {
        playerDamaged.Damaged();
        for (int i = 0; i < 60; i++)
        {
            if(i == 30)
            {
                isDamage = false;
            }
            characterController.Move(move / 60);
            yield return null;
        }
        animator.SetBool("Damaged", false);
        isMoved = true;
    }

    private void FixedUpdate()
    {
        //穴に落ちたら初期位置に戻る
        if(transform.position.y < -2.0f)
        {
            transform.position = StartPos;
            transform.rotation = Quaternion.identity;
            isMoved = true;
        }
    }

    // 移動処理
    void UpdateMove()
    {
        // テンキー、スティック入力移動処理（アクション中、被弾中、アニメーション遷移中は移動できないようにする）
        if (isMoved && !animator.IsInTransition(0))
        {
            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)  // 入力が0のとき
            {
                mover = 0.0f;
                animator.SetFloat("Speed", mover);
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
                var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(2, 0, 2));
                Vector3 direction = cameraForward * Input.GetAxisRaw("Vertical") + Camera.main.transform.right * Input.GetAxisRaw("Horizontal");  //  テンキーや3Dスティックの入力をdirectionに値を返す
                Vector3 nlz = direction.normalized;

                ChangeDirection(nlz);                       // 向き変更処理
                CharaMove(nlz, direction.magnitude);        // 移動処理
            }
        }

        //風に吹き飛ばされる処理（移動不可）
        if(isBlowAway)
        {
            mover = 0.0f;
            animator.SetFloat("Speed", mover);
            animator.SetBool("Moving", false);
            Vector3 windDirection = wind.DirectionWind; //風エリアの向きを取得

            characterController.Move(windDirection * Time.deltaTime * blowspeed); //風エリアの向きにあった方向に移動
        }
    }

    // 向き変更処理
    void ChangeDirection(Vector3 changeDirection)
    {
        Quaternion q = Quaternion.LookRotation(changeDirection);          // 向きたい方角をQuaternion型に直す
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
    }


    // 移動処理
    void CharaMove(Vector3 distanceMove, float length)
    {
        mover = Mathf.Min(length, 1);

        // プレイヤーの移動 
        characterController.Move(Mathf.Min(length, 1) * distanceMove * Time.deltaTime * charaSpeed * Resistancevalues);//キャラの移動速度に常に抵抗値を計算

        animator.SetFloat("Speed", mover);
    }

    public void ActionEnd()
    {
        Debug.Log("endaction");
        animator.SetBool("Action", false);
        isMoved = true;
        isAction = false;
    }

    //表情マネージャー
    //EMOTION ENGINE
    //ノート：表情アニメーションまだできていないので、触らないでください。(ゼン)
    public void OnCallChangeFace(string str)
    {
        /*int ichecked = 0;
    	foreach (var animation in animations) {
    		if (str == animation.name) {
    			ChangeFace (str);
    			break;
    		} else if (ichecked <= animations.Length) {
    			ichecked++;
    		} else {
    			//str指定が間違っている時にはデフォルトで
    			str = "default@unitychan";
    			ChangeFace (str);
    		}
    	} */
    }

    //ギミックに当たった時の関数(GimmickCollider.csから呼び出し)
    public void GimmickEnter(Collider other)
    {
        // BaseGimmickを取っておく
        BaseGimmick gimmick = other.GetComponent<BaseGimmick>();
        if (gimmick != null)
        {
            frontGimmick = gimmick;
            isCollision = true;
            Debug.Log("当たった : " + other.gameObject.name);
        }
        //風に当たった時に触れている風の名前と向きを取得
        if (other.gameObject.tag == "Wind")
        {
            Windname = other.gameObject.name;
            wind = other.GetComponent<WindScript>();
        }
    }

    public void GimmickExit(Collider other)
    {
        BaseGimmick gimmick = other.GetComponent<BaseGimmick>();
        if (gimmick != null)
        {
            frontGimmick = null;
            isCollision = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //リーフ回復アイテム取得
        if(other.gameObject.tag == "LeafRecovery")
        {
            Leaf += 1;
            soundEffect.Play(other.gameObject.tag);
            Destroy(other.gameObject);
        }
        //足止め機能取得フラグ
        if(other.gameObject.tag == "AttackItem")
        {
            isAttack = true;
            soundEffect.Play(other.gameObject.tag);
            Destroy(other.gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SafeArea")
        {
            isSafeArea = true;
            isBlowAway = false;
            isMoved = true;
            if (windDeltaTimer > 0)
            {
                windDeltaTimer -= wind.WindTimer * Time.deltaTime;
            }
            else
            {
                windDeltaTimer = 0;
            }
            Resistancevalues = 1.0f;
        }
        //風に当たってるとき
        if (other.gameObject.tag == "Wind" && !isSafeArea)
        {
            isWindArea = true;
            //プレイヤーの移動速度を低下させる
            Resistancevalues = 0.6f;
            //一定時間風に当たっていると吹き飛ばされる（操作不能）
            if (windDeltaTimer >= wind.WindTimer)
            {
                isBlowAway = true;
                isMoved = false;
            }
            else
            {
                windDeltaTimer += Time.deltaTime;
            }
        }
        else
        {
            isWindArea = false;
        }


        if (other.gameObject.name == "ZoomArea")
        {
            isZoomArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.name == "Cube")
        //{
        //    isCollision = false;
        //}

        //安全地帯から離れたとき
        if(other.gameObject.tag == "SafeArea")
        {
            isSafeArea = false;
        }
        //風地帯から離れたとき
        if(other.gameObject.tag == "Wind")
        {
            isBlowAway = false;
            windDeltaTimer = 0;
            Resistancevalues = 1.0f;
            isMoved = true;
        }

        if (other.gameObject.name == "ZoomArea")
        {
            isZoomArea = false;
        }
    }

    //敵からのダメージ呼び出し
    public void UpdateDamaged()
    {
        //HPが0の場合は処理しない
        if(HP <= 0)
        {
            return;
        }

        if (!animator.GetBool("Damaged") && !isBlinking)
        {
            animator.SetBool("Damaged", true);
            isDamage = true;
            isMoved = false;
            Instantiate(HitEffect, this.transform.position, Quaternion.identity);
            var back = -transform.forward;
            Vector3 direction = back * 0.2f;
            Vector3 nlz = direction.normalized;
            StartCoroutine(knockback(nlz));
        }
    }

    public bool isEndAnimation(string animname)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animname) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            return true;
        }
        return false;
    }
}
