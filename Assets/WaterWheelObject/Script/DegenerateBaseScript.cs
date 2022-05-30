using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegenerateBaseScript : MonoBehaviour
{
    //‘Ş‰»’†‚©‚Ç‚¤‚©Atrue‚È‚ç‘Ş‰»’†Afalse‚È‚ç~‚Ü‚Á‚Ä‚¢‚é
    protected bool Degenerating;

    //‰Šú‰»
    public virtual void Initialize()
    {
        Degenerating = false;
    }

    //‘Ş‰»
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
