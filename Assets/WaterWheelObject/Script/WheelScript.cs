using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    //…Ô‚Ìó‘ÔAtrue‚È‚ç…Ô‚ª“®ì‚·‚éAfalse‚È‚ç‚Â‚é‚ª—‚İ‚Â‚¢‚Ä“®ì‚µ‚È‚¢
    bool WheelState;
    //…Ô‚Ì‰ñ“]‚·‚é‘¬“xA‰½•b‚Å1‰ñ“]‚·‚é‚©‚Åw’è
    //…‚Æ‚Ì‘¬“x‚ÌŠÖŒW‚ÍRotateSpeed/WaterSpeed=2.5‚É‚È‚é‚æ‚¤‚É‚·‚é
    float RotateSpeed;
    //…Ô‚Ì‰ñ“]•ûŒüA-1,0,1‚Ì‚¢‚¸‚ê‚©‚Åw’è
    int RotateDirection;

    //‰Šú‰»
    public void Initialize()
    {
        WheelState = false;
        RotateSpeed = 5.0f;
        RotateDirection = 1;
    }

    //…Ô‚Ì‰ñ“]
    public void UpdateWheel()
    {
        //…Ô‚Ì‰ñ“]
        this.transform.Rotate(360.0f * RotateDirection / RotateSpeed * Time.deltaTime, 0f, 0f);
    }

    //Getter
    public bool GetWheelState()
    {
        return WheelState;
    }

    //Setter
    public void SetWheelState()
    {
        WheelState = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
