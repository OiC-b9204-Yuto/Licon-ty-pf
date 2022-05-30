using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_WINDScript : MonoBehaviour
{
    public bool FX_Wind;

    private void Start()
    {
        FX_Wind = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (FX_Wind)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }
}

