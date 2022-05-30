using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceWaterScript : MonoBehaviour
{
    //���̒���
    protected float WaterSize;
    //���̑���
    protected float WaterSpeed;
    //���������Ă��邩�Atrue�Ȃ瓮���Ă���Afalse�Ȃ�~�܂��Ă���
    protected bool WaterMove;

    //������
    public virtual void Initialize()
    {
        WaterSize = 1.5f;
        WaterSpeed = 0.5f;
        WaterMove = true;
    }

    //����鐅
    public virtual void UpdateWater()
    {
        WaterSize += WaterSpeed * Time.deltaTime;
        this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, WaterSize);
    }

    //WaterStop�I�u�W�F�N�g�ƏՓ˂����ꍇ�A����傫������̂��~�߂�
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WaterStop")
        {
            WaterMove = false;
        }
    }

    //Getter
    public bool GetWaterMove()
    {
        return WaterMove;
    }

    //Setter
    public void SetWaterMove()
    {
        WaterMove = true;
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
