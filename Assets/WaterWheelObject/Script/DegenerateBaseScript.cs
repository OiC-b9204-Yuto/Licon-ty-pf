using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegenerateBaseScript : MonoBehaviour
{
    //�މ������ǂ����Atrue�Ȃ�މ����Afalse�Ȃ�~�܂��Ă���
    protected bool Degenerating;

    //������
    public virtual void Initialize()
    {
        Degenerating = false;
    }

    //�މ�
    public virtual void Degeneration()
    {

    }

    //Getter
    public bool GetDegenerating()
    {
        return Degenerating;
    }

    //Setter
    public void SetDegenerating()
    {
        Degenerating = true;
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
