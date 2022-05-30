using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utl_ObjRotator : MonoBehaviour
{
    
	public Vector3 rotateVector;
	public bool pulsate = false;
	public float pulsateSpeed = 5f;
	public float pulsateHeight = .5f;
	public float floorOffset = 2f;

	private Vector3 initPos;
	
	void Start(){
		if(pulsate) initPos = transform.position;
	}
	
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateVector * Time.deltaTime);
		
		if(pulsate){
			
			float newY = Mathf.Sin(Time.time * pulsateSpeed) + (initPos.y + floorOffset);
			
			transform.position = new Vector3(initPos.x,newY* pulsateHeight,initPos.z);
		}
    }
}
