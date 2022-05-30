using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Enemy;

public class ArrowEnemy : MonoBehaviour, IStuck
{
    [SerializeField] private float m_ArrowSpeed = 0.08f;
    [SerializeField] private float m_GenerationSpan = 1.5f;
    [SerializeField] private float m_ArrowLifeSpan = 10.0f;

    [SerializeField] private int m_RapidCount = 1;
    [SerializeField] private float m_RapidSpan = 0.1f;

    [SerializeField] private bool fixedDirFlg = false;
    [SerializeField] private bool m_AnimationMatchFlg = true;

    [SerializeField, Range(1, 360)] private int m_SplitDirCount = 4;
    [SerializeField, Range(0, 360)] private float fixedDir = 0;

    private float m_GeneratedTime = 0;

    private Transform PlayerTransform;

    private bool m_bBind = false;

    private Animator animator;

    [SerializeField] private GameObject obj;

    private bool m_bFired = false;
    private int m_RapidNo = 0;

    [SerializeField] private AudioClip sound;

    AudioSource audioSource;

    public bool IsStuck()
    {
        return m_bBind;
    }

    public void TakeStuck()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
        m_bBind = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GeneratedTime = Time.time;
        Vector3 pos = transform.position;
        pos.y = Mathf.Ceil(pos.y);
        pos.x = Mathf.Round(pos.x);
        pos.z = Mathf.Round(pos.z);
        pos.x = pos.x + 0.5f;
        pos.z = pos.z + 0.5f;
        transform.position = pos;

        m_RapidNo = m_RapidCount;

        animator = GetComponent<Animator>();
        animator.SetBool("isAttacking", true);

        audioSource = GetComponent<AudioSource>();

        PlayerTransform = GameObject.Find("Player").transform;
    }

    private float GetArrowDirection()
    {
        Vector3 Rad = PlayerTransform.position - transform.position;
        // 角度を求める 0~360
        float Dir = Mathf.Atan2(Rad.z, Rad.x) * Mathf.Rad2Deg + 180;
        // 飛ぶ方角を(360 / 方角の数)で割る
        float Interval = 360 / m_SplitDirCount;
        // 現在の方角を飛ぶべき角で割る
        int direction = (int)((Dir + Interval / 2) / Interval);
        // 割った値をあるべき角度になおす -180~180
        Dir = direction * Interval - 180;
        return Dir;
    }

    private void ArrowFire()
    {
        Vector3 Pos = transform.position;
        Pos.y += 0.5f;
        GameObject arrow = Instantiate(obj, Pos, Quaternion.identity);
        if (!fixedDirFlg)
        {    
            arrow.GetComponent<Arrow>().Initialize(m_ArrowSpeed, GetArrowDirection() * Mathf.Deg2Rad, m_ArrowLifeSpan);
        }
        else
        {
            arrow.GetComponent<Arrow>().Initialize(m_ArrowSpeed, fixedDir * Mathf.Deg2Rad, m_ArrowLifeSpan);
        }
        audioSource.PlayOneShot(sound);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bBind) { return; }
        if (!fixedDirFlg)
        {
            // エネミーの向いている方向を変える
            // 現在の向きを取得
            Vector3 rot = transform.localEulerAngles;
            // 向きを発射方向に変更
            rot.y = -GetArrowDirection() + 90;
            // 変更を保存
            transform.localEulerAngles = rot;
        }
        else
        {
            // エネミーの向いている方向を変える
            // 現在の向きを取得
            Vector3 rot = transform.localEulerAngles;
            // 向きを発射方向に変更
            rot.y = -fixedDir + 90;
            // 変更を保存
            transform.localEulerAngles = rot;
        }

        bool fire = false;
        if (m_AnimationMatchFlg)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] Clip = animator.GetCurrentAnimatorClipInfo(0);
            float time = Clip[0].clip.length * state.normalizedTime;

            int AllTime = (int)(state.length * 100 * state.speed);
            int Time = (int)(time * 100);

            if (Time % AllTime <= 350)
            {
                m_bFired = false;
            }
            if (!m_bFired && Time % AllTime >= 350)
            {
                m_bFired = true;
                fire = true;
            }
        }
        else
        {
            if (Time.time - m_GeneratedTime > m_GenerationSpan)
            {
                fire = true;
            }
        }

        if (fire || m_RapidNo < m_RapidCount)
        {
            if (Time.time - m_GeneratedTime > m_RapidSpan)
            {
                ArrowFire();
                m_GeneratedTime = Time.time;
                m_RapidNo--;
                if (m_RapidNo <= 0)
                {
                    m_RapidNo = m_RapidCount;
                }
            }
        }
    }
}
