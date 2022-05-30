using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 10, 0);
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
