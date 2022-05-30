using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    //���Ԃ̏�ԁAtrue�Ȃ琅�Ԃ����삷��Afalse�Ȃ�邪���݂��ē��삵�Ȃ�
    bool WheelState;
    //���Ԃ̉�]���鑬�x�A���b��1��]���邩�Ŏw��
    //���Ƃ̑��x�̊֌W��RotateSpeed/WaterSpeed=2.5�ɂȂ�悤�ɂ���
    float RotateSpeed;
    //���Ԃ̉�]�����A-1,0,1�̂����ꂩ�Ŏw��
    int RotateDirection;

    //������
    public void Initialize()
    {
        WheelState = false;
        RotateSpeed = 5.0f;
        RotateDirection = 1;
    }

    //���Ԃ̉�]
    public void UpdateWheel()
    {
        //���Ԃ̉�]
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
