using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionEngine : MonoBehaviour
{
	//[Header ("Index")]
	public int px,nx,py,ny,blinkIndex;
	
	[Header("Blink 182")]
	public bool blinkable = true;
	public float blinkInterval = 3.0f;
	public float blinkSpeed = 0.3f;
	
	[Header("Eye Controller")]
	public Vector2 eyeController;
	public float targetMultiplier = 100f;
	
	[Header("Debugger")]
	public bool isDebugging = false;

	public string pxName;
	public string nxName;
	public string pyName;
	public string nyName;
	public string blinkerName;
	
    private SkinnedMeshRenderer smr;
	private float blinkVal = 0f;
	
	private Vector4 twoDeePos;
	//private float t = 0f;

	void Start(){
		smr = GetComponent<SkinnedMeshRenderer>();
		
		if(blinkable && smr) StartCoroutine(Blink());
	}
	
    // Update is called once per frame
    void LateUpdate()
    {
        if(eyeController.x >0 || eyeController.y >0){ //Positive Converter
			twoDeePos.x = eyeController.x;
			twoDeePos.y = eyeController.y;
		}

		if(eyeController.x < 0 || eyeController.y < 0){
			twoDeePos.z = -eyeController.x;
			twoDeePos.w = -eyeController.y;
		}
		
		eyeController.x = Mathf.Clamp(eyeController.x, -1f,1f);
		eyeController.y = Mathf.Clamp(eyeController.y, -1f,1f);
		
		smr.SetBlendShapeWeight(px,twoDeePos.x * targetMultiplier);
		smr.SetBlendShapeWeight(py,twoDeePos.y * targetMultiplier);
		smr.SetBlendShapeWeight(nx,twoDeePos.z * targetMultiplier);
		smr.SetBlendShapeWeight(ny,twoDeePos.w * targetMultiplier);
    }
	
	IEnumerator Blink(){
		
		
		while(true){
			blinkVal = 0f;
			smr.SetBlendShapeWeight(blinkIndex, blinkVal);
			
			yield return new WaitForSeconds(blinkInterval);
			
			while(blinkVal < 99f){
				//blinkVal = Mathf.Lerp(blinkVal, 100f, t / blinkSpeed);
				//t += Time.deltaTime;
				blinkVal += Time.deltaTime * blinkSpeed;
				blinkVal = Mathf.Clamp(blinkVal, 0f,100f);
				smr.SetBlendShapeWeight(blinkIndex, blinkVal);
				//print(blinkVal + " | "  + smr.GetBlendShapeWeight(blinkIndex));
				yield return null;
			}
			
			blinkVal = 100f;
			smr.SetBlendShapeWeight(blinkIndex, blinkVal);
			yield return new WaitForSeconds(.05f);
			
			while(blinkVal > 0f){
				//blinkVal = Mathf.Lerp(blinkVal, 100f, t / blinkSpeed);
				//t += Time.deltaTime;
				blinkVal -= Time.deltaTime * blinkSpeed;
				blinkVal = Mathf.Clamp(blinkVal, 0f,100f);
				smr.SetBlendShapeWeight(blinkIndex, blinkVal);
				//print(blinkVal + " | "  + smr.GetBlendShapeWeight(blinkIndex));
				yield return null;
			}
			
			//print("LMAO");
			
			/*while(smr.GetBlendShapeWeight(blinkIndex) > 0f){
				blinkVal = Mathf.Lerp(blinkVal, 0, blinkSpeed);
				smr.SetBlendShapeWeight(blinkIndex, blinkVal);
			}*/
		}
	}
}
