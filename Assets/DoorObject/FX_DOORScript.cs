using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_DOORScript : MonoBehaviour
{
    public bool FX_DOOR;

    private void Start()
    {
        FX_DOOR = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(FX_DOOR)
        {
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }
}
