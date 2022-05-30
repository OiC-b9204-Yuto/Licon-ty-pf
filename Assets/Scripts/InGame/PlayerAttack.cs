using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Enemy;

public class PlayerAttack : MonoBehaviour
{
    //標準用のオブジェクト
    public GameObject AimObject;
    //標準用のエフェクト
    [SerializeField]
    private GameObject AimEffect;
    private GameObject InstanceEffect;
    //エイム中かどうか
    public bool isAiming { get; set; }
    //スキル発動中判定
    public bool isSkillActive { get; set; }
    //エイムの速さ
    private float AimSpeed = 300.0f;
    //クールタイム
    public const int CoolTime = 10;
    //弾関連
    private float VineSpeed = 5.0f;
    private float VineShowTime = 5.0f;
    private bool isShot = false;
    private Vector3 Direction;
    //スキルのクールダウン判定
    public bool isCoolDown { get; private set; }
    //アニメーションコンポーネント用変数
    private Animator animator;
    //エフェクト発生フラグ
    private bool isShowEffect;
    //拘束用
    private IStuck stuck;

    // Start is called before the first frame update
    void Start()
    {
        isAiming = false;
        isCoolDown = false;
        isSkillActive = false;
        animator = GetComponent<Animator>();
        AimObject.transform.position = GetAimPosition();
        isShowEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAiming && !isCoolDown)
        {

            AimObject.SetActive(true);
            animator.SetBool("Aiming", true);
            if(Input.GetAxisRaw("RVertical") != 0 || Input.GetAxisRaw("RHorizontal") != 0)
            {
                var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(2, 0, 2));
                Vector3 direction = cameraForward * Input.GetAxisRaw("RVertical") + Camera.main.transform.right * Input.GetAxisRaw("RHorizontal");  //  テンキーや3Dスティックの入力をdirectionに値を返す
                Vector3 nlz = direction.normalized;
                ChangeAimDirection(nlz);         // 方向変更
            }
            if (Input.GetButtonDown("ShotVine"))
            {
                animator.SetBool("Action", true);
                isSkillActive = true;
                VineShot();
            }
        }
        else
        {
            animator.SetBool("Aiming", false);
            if(!isShot)
            {
                AimObject.transform.position = GetAimPosition();
                AimObject.SetActive(false);
            }

        }

        if (AimObject.activeSelf)
        {
            if(!isShowEffect)
            {
                var instantiateEffect = GameObject.Instantiate(AimEffect, AimObject.transform.position, Quaternion.identity, AimObject.transform) as GameObject;
                InstanceEffect = instantiateEffect;
                isShowEffect = true;
            }
        }
        else
        {
            if(isShowEffect)
            {
                Destroy(InstanceEffect);
                isShowEffect = false;
            }
        }

        if(isShot)
        {
            AimObject.transform.position += Direction * VineSpeed * Time.deltaTime;
            if(VineShowTime > 0)
            {
                VineShowTime -= Time.deltaTime;
            }
            else
            {
                isShot = false;
                VineShowTime = 5.0f;
            }
        }

    }

    //エイム中にデバッグテキストをコンソール上に表示させる
    public void RenderAimingText()
    {
        if(isAiming && !isCoolDown)
        {
            Debug.Log("Aim中");
        }
        else
        {
            Debug.Log("クールダウン中");
        }
    }

    // 標準の方向変更 
    void ChangeAimDirection(Vector3 changeDirection)
    {
        Quaternion q = Quaternion.LookRotation(changeDirection);          // 向きたい方角をQuaternion型に直す
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, q, AimSpeed * Time.deltaTime);
        AimObject.transform.position = GetAimPosition();
        InstanceEffect.transform.position = AimObject.transform.position;
    }

    // 攻撃処理
    void VineShot()
    {
        isShot = true;
        Direction = this.transform.forward;
        StartCoroutine(SkillCoolTime());
        if (stuck != null)
        {
            stuck.TakeStuck();
        }
    }

    // 足止めのクールダウン
    IEnumerator SkillCoolTime()
    {
        isCoolDown = true;

        //yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < CoolTime; i++)
        {
            yield return new WaitForSeconds(1.0f);
            if (i == 1)
            {
                isSkillActive = false;
                animator.SetBool("Action", false);
            }
        }

        isCoolDown = false;
    }

    //座標取得処理
    Vector3 GetAimPosition()
    {
        return this.transform.position + new Vector3(0, 0.75f, 0) + this.transform.forward * 0.5f;
    }

    public void AimTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            stuck = other.GetComponent<IStuck>();
            AimObject.SetActive(false);
        }
    }

    public void AimTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            stuck = null;
        }
    }
}

