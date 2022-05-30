using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        transform.root.gameObject.GetComponent<PlayerMove>().GimmickEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        transform.root.gameObject.GetComponent<PlayerMove>().GimmickExit(other);
    }
}
