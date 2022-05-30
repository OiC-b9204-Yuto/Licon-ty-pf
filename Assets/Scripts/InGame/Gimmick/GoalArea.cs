using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Licon.Gimmick
{
    /// <summary>
    /// ゴール判定用
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class GoalArea : MonoBehaviour
    {
        private Collider _collider;
        private Rigidbody _rigidbody;

        public bool isGoal { get; private set; }
        void Reset()
        {
            _collider = this.GetComponent<Collider>();
            _collider.isTrigger = true;
            _rigidbody = this.GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            isGoal = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            //タグがプレイヤーならゴールしたとして
            if (other.tag == "Player")
            {
                isGoal = true;
            }
        }
    }
}