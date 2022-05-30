using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObjectCollider : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        player.GetComponent<PlayerAttack>().AimTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        player.GetComponent<PlayerAttack>().AimTriggerExit(other);
    }
}
