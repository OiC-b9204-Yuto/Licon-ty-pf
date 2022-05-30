using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEmissive : MonoBehaviour
{
    MeshRenderer meshRenderer;
    ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = this.GetComponentInChildren<ParticleSystem>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        DisabledEmission();
    }

    public void ActivateEmission()
    {
        if(particleSystem)
        {
            particleSystem.Play();
        }
        meshRenderer.material.SetFloat("_EMISSIVE", 1.0f);
        meshRenderer.material.EnableKeyword("_EMISSIVE_ANIMATION");
        meshRenderer.material.DisableKeyword("_EMISSIVE_SIMPLE");
    }

    public void DisabledEmission()
    {
        if (particleSystem)
        {
            particleSystem.Stop();
        }
        meshRenderer.material.SetFloat("_EMISSIVE", 0.0f);
        meshRenderer.material.EnableKeyword("_EMISSIVE_SIMPLE");
        meshRenderer.material.DisableKeyword("_EMISSIVE_ANIMATION");
    }
}
