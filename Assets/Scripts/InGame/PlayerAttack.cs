using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Enemy;

public class PlayerAttack : MonoBehaviour
{
    //�W���p�̃I�u�W�F�N�g
    public GameObject AimObject;
    //�W���p�̃G�t�F�N�g
    [SerializeField]
    private GameObject AimEffect;
    private GameObject InstanceEffect;
    //�G�C�������ǂ���
    public bool isAiming { get; set; }
    //�X�L������������
    public bool isSkillActive { get; set; }
    //�G�C���̑���
    private float AimSpeed = 300.0f;
    //�N�[���^�C��
    public const int CoolTime = 10;
    //�e�֘A
    private float VineSpeed = 5.0f;
    private float VineShowTime = 5.0f;
    private bool isShot = false;
    private Vector3 Direction;
    //�X�L���̃N�[���_�E������
    public bool isCoolDown { get; private set; }
    //�A�j���[�V�����R���|�[�l���g�p�ϐ�
    private Animator animator;
    //�G�t�F�N�g�����t���O
    private bool isShowEffect;
    //�S���p
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
                Vector3 direction = cameraForward * Input.GetAxisRaw("RVertical") + Camera.main.transform.right * Input.GetAxisRaw("RHorizontal");  //  �e���L�[��3D�X�e�B�b�N�̓��͂�direction�ɒl��Ԃ�
                Vector3 nlz = direction.normalized;
                ChangeAimDirection(nlz);         // �����ύX
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

    //�G�C�����Ƀf�o�b�O�e�L�X�g���R���\�[����ɕ\��������
    public void RenderAimingText()
    {
        if(isAiming && !isCoolDown)
        {
            Debug.Log("Aim��");
        }
        else
        {
            Debug.Log("�N�[���_�E����");
        }
    }

    // �W���̕����ύX 
    void ChangeAimDirection(Vector3 changeDirection)
    {
        Quaternion q = Quaternion.LookRotation(changeDirection);          // �����������p��Quaternion�^�ɒ���
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, q, AimSpeed * Time.deltaTime);
        AimObject.transform.position = GetAimPosition();
        InstanceEffect.transform.position = AimObject.transform.position;
    }

    // �U������
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

    // ���~�߂̃N�[���_�E��
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

    //���W�擾����
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

