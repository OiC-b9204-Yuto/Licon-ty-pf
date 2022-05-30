using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : DegenerateBaseScript
{
    //‰Šú‰»
    public override void Initialize()
    {
        base.Initialize();
        this.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }

    //‘Ş‰»
    public override void Degeneration()
    {
        //–Ø‚Ì‘Ş‰»(k¬)
        this.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
        this.gameObject.transform.localPosition -= new Vector3(0.0f, 0.3f, 1.0f) * Time.deltaTime;

        //–Ø‚Ì‘å‚«‚³‚ª0.1–¢–‚É‚È‚Á‚½‚ç‘Ş‰»‚ğ~‚ß‚é
        if (this.transform.localScale.y < 0.1f)
        {
            this.gameObject.SetActive(false);
            Degenerating = false;
        }
    }

    //Tree‚ÌlocalScale.y‚ÌGetter
    public float GetTreeScale()
    {
        return this.gameObject.transform.localScale.y;
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
