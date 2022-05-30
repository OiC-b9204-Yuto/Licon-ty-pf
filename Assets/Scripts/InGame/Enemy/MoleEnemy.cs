using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Enemy
{
    public class MoleEnemy : EnemyBase
    {
        [SerializeField] private GameObject player;

        [SerializeField] private float moveSpeed = 4;

        //[SerializeField] private List<Vector3> patrolPoints = new List<Vector3>();
        [SerializeField] /*[Tooltip("移動可能範囲の最小値")]*/ private Vector3 minPosition;
        [SerializeField] /*[Tooltip("移動可能範囲の最大値")]*/ private Vector3 maxPosition;

        [SerializeField] private float stateChangeTime;

        [Serializable] 
        private struct Range
        {
            public float min;
            public float max;
            public Range(float min,float max)
            {
                this.min = min;
                this.max = max;
            }
        }

        [SerializeField] private Range timeRandomRange = new Range(10.0f,10.0f);
        private Transform targetObject;

        private Rigidbody _rigidbody;
        private Animator _animator;

        [SerializeField] Vector3 moveVector = new Vector3();
        private bool turnFlag;

        [SerializeField] private GameObject trailObject;
        [SerializeField] private GameObject holeObject;

        enum MoleEnemyState
        {
            Idle,
            Patrol,
            Chase,
            Stuck
        }

        [SerializeField] private MoleEnemyState myState = MoleEnemyState.Idle;
        private void ChengeState(MoleEnemyState state)
        {
            if (myState == state)
            {
                return;
            }

            switch (state)
            {
                case MoleEnemyState.Idle:
                    break;
                case MoleEnemyState.Patrol:
                    break;
                case MoleEnemyState.Chase:
                    break;
                case MoleEnemyState.Stuck:
                    break;
            }

            myState = state;
        }

        private void Awake()
        {
            //プレイヤーのGameObject取得
            player = GameObject.Find("Player");
            if (player && player.tag == "Player")
            {
                targetObject = player.transform;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("プレイヤーが見つかりませんでした。");
            }
#endif
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            Vector2 range = TurnRadianRange();
            float rad = UnityEngine.Random.Range(range.x, range.y);
            moveVector = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        void Update()
        {
            stateChangeTime -= Time.deltaTime;
            switch (myState)
            {
                case MoleEnemyState.Idle:
                    IdleUpdate();
                    break;
                case MoleEnemyState.Patrol:
                    PatrolUpdate();
                    break;
                case MoleEnemyState.Chase:
                    ChaseUpdate();
                    break;
                case MoleEnemyState.Stuck:
                    StuckUpdate();
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (myState)
            {
                case MoleEnemyState.Idle:
                    IdleFixedUpdate();
                    break;
                case MoleEnemyState.Patrol:
                    PatrolFixedUpdate();
                    break;
                case MoleEnemyState.Chase:
                    ChaseFixedUpdate();
                    break;
                case MoleEnemyState.Stuck:
                    StuckFixedUpdate();
                    break;
            }
        }

        private void IdleUpdate()
        {
            if (stateChangeTime <= 0 || (_animator.GetCurrentAnimatorStateInfo(0).IsName("PEEK") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f))
            {
                _animator.SetBool("isPeeking", false);
                ChengeState(MoleEnemyState.Patrol);
                stateChangeTime = UnityEngine.Random.Range(timeRandomRange.min, timeRandomRange.max);
                trailObject.SetActive(true);
                holeObject.SetActive(false);
            }
        }

        private void IdleFixedUpdate()
        {

        }

        private void PatrolUpdate()
        {
            if (turnFlag)
            {
                Vector2 range = TurnRadianRange();
                float rad = UnityEngine.Random.Range(range.x, range.y);
                moveVector = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
                transform.rotation = Quaternion.LookRotation(moveVector);
                turnFlag = false;
            }

            if (stateChangeTime <= 0)
            {
                _rigidbody.velocity = Vector3.zero;
                ChengeState(MoleEnemyState.Idle);
                stateChangeTime = 5;
                trailObject.SetActive(false);
                holeObject.SetActive(true);
                _animator.SetBool("isPeeking", true);
            }
        }

        private void PatrolFixedUpdate()
        {
            if (transform.position.x < minPosition.x)
            {
                moveVector.x = -moveVector.x;
                transform.position = new Vector3(minPosition.x + minPosition.x - transform.position.x, transform.position.y, transform.position.z);
                turnFlag = true;
            }
            else if (transform.position.x > maxPosition.x)
            {
                moveVector.x = -moveVector.x;
                transform.position = new Vector3(maxPosition.x + maxPosition.x - transform.position.x, transform.position.y, transform.position.z);
                turnFlag = true;
            }

            if (transform.position.z < minPosition.z)
            {
                moveVector.z = -moveVector.z;
                transform.position = new Vector3(transform.position.x, transform.position.y, minPosition.z + minPosition.z - transform.position.z);
                turnFlag = true;
            }
            else if (transform.position.z > maxPosition.z)
            {
                moveVector.z = -moveVector.z;
                transform.position = new Vector3(transform.position.x, transform.position.y, maxPosition.z + maxPosition.z - transform.position.z);
                turnFlag = true;
            }

            _rigidbody.velocity = moveVector * moveSpeed;
        }

        private void ChaseUpdate()
        {

        }

        private void ChaseFixedUpdate()
        {
            Vector3 vec3 = targetObject.position - this.transform.position;
            vec3.y = 0;
            vec3.Normalize();
            _rigidbody.velocity = vec3 * moveSpeed;
        }

        private void StuckUpdate()
        {
            
        }

        private void StuckFixedUpdate()
        {

        }

        public override void TakeStuck()
        {
            if (myState == MoleEnemyState.Idle) {
                stuck = true;
                myState = MoleEnemyState.Stuck;
                _animator.SetBool("isDead", true);
                _rigidbody.velocity = Vector3.zero;
                this.GetComponent<Collider>().isTrigger = false;
            }
        }

        private Vector2 TurnRadianRange()
        {
            Vector2 ret = new Vector2(0.05f, Mathf.PI * 2 - 0.05f);

            Vector2 min = new Vector2(Mathf.Abs(minPosition.x - transform.position.x), Mathf.Abs(minPosition.z - transform.position.z));
            Vector2 max = new Vector2(Mathf.Abs(maxPosition.x - transform.position.x), Mathf.Abs(maxPosition.z - transform.position.z));

            if (min.y < 1)
            {
                ret.y -= Mathf.PI;
                if (min.x < 1)
                {
                    ret.y -= Mathf.PI * 0.5f;
                }
                else if (max.x < 1)
                {
                    ret.x += Mathf.PI * 0.5f;
                }
            }
            else if(max.y < 1)
            {
                ret.x += Mathf.PI;
                if (min.x < 1)
                {
                    ret.x += Mathf.PI * 0.5f;
                }
                else if (max.x < 1)
                {
                    ret.y -= Mathf.PI * 0.5f;
                }
            }
            else
            {
                if (min.x < 1)
                {
                    ret.x -= Mathf.PI * 0.5f;
                    ret.y -= Mathf.PI * 1.5f;
                }
                else if (max.x < 1)
                {
                    ret.x += Mathf.PI * 0.5f;
                    ret.y -= Mathf.PI * 0.5f;
                }
            }
            return ret;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (stuck)
            {
                return;
            }

            if (collider.gameObject.tag == "Player")
            {
                var player = collider.gameObject.GetComponent<PlayerMove>();
                if (player)
                {
                    player.UpdateDamaged();
                }
            }
        }

#if UNITY_EDITOR
        [ContextMenu("PatrolArea Reset")]
        private void PatrolAreaReset()
        {
            minPosition = transform.position + new Vector3(-5, 0, -5);
            minPosition.y = 0;
            maxPosition = transform.position + new Vector3(5, 0, 5);
            maxPosition.y = 0;
        }


        void OnDrawGizmosSelected()
        {
            Vector3 size = maxPosition - minPosition;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(maxPosition - size * 0.5f, size);
        }
#endif
    }
}
