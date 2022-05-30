using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utl_AnimationEvent : MonoBehaviour
{
	
	//public UnityEvents[] subjects;
	
	public GameObject[] actors;
	public ParticleSystem[] particles;
	
	public Vector3[] actors_vectors;
	
    public void AE_InstantiateObject(int index){
		Instantiate(actors[index],transform.position, Quaternion.Euler(actors_vectors[index]));
	}
	
	public void AE_ParticlesPlay(int index){
		particles[index].Play();
	}
	
	public void AE_ParticlesStop(int index){
		particles[index].Stop();
	}
	
	public void AE_TurnObjectOn(int index){
		actors[index].SetActive(true);
	}
	
	public void AE_TurnObjectOff(int index){
		actors[index].SetActive(false);
	}
}
