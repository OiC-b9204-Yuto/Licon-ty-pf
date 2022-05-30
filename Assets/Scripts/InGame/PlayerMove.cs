using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Gimmick;
using Licon.Damaged;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] UIInGame ui;
    private CharacterController characterController;    // �L�����N�^�[�R���|�[�l���g�p�ϐ�
    private Animator animator;                          //�A�j���[�V�����R���|�[�l���g�p�ϐ�

    public float charaSpeed = 5.0f;         // �L�����N�^�[�̃X�s�[�h
    public float rotateSpeed = 1200.0f;     // �L�����N�^�[�̉�]�X�s�[�h
    public float mover = 0.0f;              // �L�����N�^�[�̓��͗�
    public float interval = 0.1f;
    [SerializeField]
    private GameObject HitEffect;           //�Փˎ��G�t�F�N�g

    //private float hitWindTimer = 0;         //���ɓ������Ă���b��
    //private float blowAwayTimer = 8.0f;     //���ɐ�����΂����܂ł̕b��
    public float windDeltaTimer { get; private set; }       //���^�C�}�[��deltaTime�p�̕ϐ�

    private bool isMoved;                   //�ړ��ۃt���O
    public bool isWindArea { get; private�@set; }                //���G���A�ɂ��邩�̃t���O
    private bool isBlowAway;                // ���ɐ�����΂���Ă��邩�̃t���O�i�L����Ԃ̏ꍇ�͈ړ��ł��Ȃ��j
    private bool isSafeArea;                //���G���A���ň��S�n�тɂ��邩�ǂ����̃t���O

    private float Resistancevalues;         //��R�l�p
    private string Windname;                //�G��Ă��镗�̖��O�擾�p
    private float blowspeed;                //�v���C���[�𐁂���΂��ۂ̕����p�ϐ��i���u��10.0f�j

    public bool isAttack { get; private set; }                  //�U���ۃt���O

    //�L�����N�^�[��HP
    public int HP { get; set; }

    //�L�����N�^�[�̃��[�t�̐�
    public int Leaf { get; private set; }

    //�����蔻��
    public bool isCollision { get; private set; }
    
    //�_�Œ�
    public bool isBlinking { get; private set; }

    //�_���[�W���󂯂�
    public bool isDamage { get; private set; }

    //�A�N�V������(�M�~�b�N������)
    public bool isAction { get; set; }

    //�����J�����͈͓̔���
    public bool isZoomArea { get; private set; }

    //�ڂ̑O�ɂ���M�~�b�N�̎Q��
    public BaseGimmick frontGimmick { get; private set; }

    //�_���[�W�������s�����߂̃X�N���v�g�ϐ�
    private PlayerDamaged playerDamaged;

    //���p
    private WindScript wind;

    //�v���C���[�̍U���X�N���v�g�ϐ�
    private PlayerAttack playerAttack;

    //�����n�_
    private Vector3 StartPos;

    //�S�[���G���A�X�N���v�g�ϐ�
    private GameObject goalObject;
    private GoalArea goalArea;

    //���ʉ��X�N���v�g�ϐ�
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
        //�|�[�Y����return
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

        //�ړ�����
        UpdateMove();

        //�A�N�V��������
        if (isCollision)
        {
            if (frontGimmick.ConditionCheck())
            {
                //�M�~�b�N�𔭓�������
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

        //�_���[�W����
        isBlinking = playerDamaged.isDamaged;

        //�U������
        if(Input.GetButton("Aim"))
        {
            //�A�C�e���擾�O�͍U���ł��Ȃ��悤�ɂ���
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
        //���ɗ������珉���ʒu�ɖ߂�
        if(transform.position.y < -2.0f)
        {
            transform.position = StartPos;
            transform.rotation = Quaternion.identity;
            isMoved = true;
        }
    }

    // �ړ�����
    void UpdateMove()
    {
        // �e���L�[�A�X�e�B�b�N���͈ړ������i�A�N�V�������A��e���A�A�j���[�V�����J�ڒ��͈ړ��ł��Ȃ��悤�ɂ���j
        if (isMoved && !animator.IsInTransition(0))
        {
            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)  // ���͂�0�̂Ƃ�
            {
                mover = 0.0f;
                animator.SetFloat("Speed", mover);
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
                var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(2, 0, 2));
                Vector3 direction = cameraForward * Input.GetAxisRaw("Vertical") + Camera.main.transform.right * Input.GetAxisRaw("Horizontal");  //  �e���L�[��3D�X�e�B�b�N�̓��͂�direction�ɒl��Ԃ�
                Vector3 nlz = direction.normalized;

                ChangeDirection(nlz);                       // �����ύX����
                CharaMove(nlz, direction.magnitude);        // �ړ�����
            }
        }

        //���ɐ�����΂���鏈���i�ړ��s�j
        if(isBlowAway)
        {
            mover = 0.0f;
            animator.SetFloat("Speed", mover);
            animator.SetBool("Moving", false);
            Vector3 windDirection = wind.DirectionWind; //���G���A�̌������擾

            characterController.Move(windDirection * Time.deltaTime * blowspeed); //���G���A�̌����ɂ����������Ɉړ�
        }
    }

    // �����ύX����
    void ChangeDirection(Vector3 changeDirection)
    {
        Quaternion q = Quaternion.LookRotation(changeDirection);          // �����������p��Quaternion�^�ɒ���
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
    }


    // �ړ�����
    void CharaMove(Vector3 distanceMove, float length)
    {
        mover = Mathf.Min(length, 1);

        // �v���C���[�̈ړ� 
        characterController.Move(Mathf.Min(length, 1) * distanceMove * Time.deltaTime * charaSpeed * Resistancevalues);//�L�����̈ړ����x�ɏ�ɒ�R�l���v�Z

        animator.SetFloat("Speed", mover);
    }

    public void ActionEnd()
    {
        Debug.Log("endaction");
        animator.SetBool("Action", false);
        isMoved = true;
        isAction = false;
    }

    //�\��}�l�[�W���[
    //EMOTION ENGINE
    //�m�[�g�F�\��A�j���[�V�����܂��ł��Ă��Ȃ��̂ŁA�G��Ȃ��ł��������B(�[��)
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
    			//str�w�肪�Ԉ���Ă��鎞�ɂ̓f�t�H���g��
    			str = "default@unitychan";
    			ChangeFace (str);
    		}
    	} */
    }

    //�M�~�b�N�ɓ����������̊֐�(GimmickCollider.cs����Ăяo��)
    public void GimmickEnter(Collider other)
    {
        // BaseGimmick������Ă���
        BaseGimmick gimmick = other.GetComponent<BaseGimmick>();
        if (gimmick != null)
        {
            frontGimmick = gimmick;
            isCollision = true;
            Debug.Log("�������� : " + other.gameObject.name);
        }
        //���ɓ����������ɐG��Ă��镗�̖��O�ƌ������擾
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
        //���[�t�񕜃A�C�e���擾
        if(other.gameObject.tag == "LeafRecovery")
        {
            Leaf += 1;
            soundEffect.Play(other.gameObject.tag);
            Destroy(other.gameObject);
        }
        //���~�ߋ@�\�擾�t���O
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
        //���ɓ������Ă�Ƃ�
        if (other.gameObject.tag == "Wind" && !isSafeArea)
        {
            isWindArea = true;
            //�v���C���[�̈ړ����x��ቺ������
            Resistancevalues = 0.6f;
            //��莞�ԕ��ɓ������Ă���Ɛ�����΂����i����s�\�j
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

        //���S�n�т��痣�ꂽ�Ƃ�
        if(other.gameObject.tag == "SafeArea")
        {
            isSafeArea = false;
        }
        //���n�т��痣�ꂽ�Ƃ�
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

    //�G����̃_���[�W�Ăяo��
    public void UpdateDamaged()
    {
        //HP��0�̏ꍇ�͏������Ȃ�
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
