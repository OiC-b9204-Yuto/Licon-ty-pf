using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utl_VideoResetter : MonoBehaviour
{
	UnityEngine.Video.VideoPlayer vp;
	
    void Awake(){
		vp = GetComponent<UnityEngine.Video.VideoPlayer>();
		vp.targetTexture.Release();
	}
}
