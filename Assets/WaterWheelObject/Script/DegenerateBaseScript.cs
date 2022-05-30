using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegenerateBaseScript : MonoBehaviour
{
    //退化中かどうか、trueなら退化中、falseなら止まっている
    protected bool Degenerating;

    //初期化
    public virtual void Initialize()
    {
        Degenerating = false;
    }

    //退化
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
