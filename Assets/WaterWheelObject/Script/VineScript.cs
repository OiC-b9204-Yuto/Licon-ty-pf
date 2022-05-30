using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineScript : DegenerateBaseScript
{

    public override void Degeneration()
    {
        //‚Â‚é‚Ì‘Þ‰»
        this.gameObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;

        //‚Â‚é‚Ì‘å‚«‚³‚ª0.1–¢–ž‚É‚È‚Á‚½‚ç‘Þ‰»‚ðŽ~‚ß‚é
        if (this.transform.localScale.y < 0.1f)
        {
            this.gameObject.SetActive(false);
            Degenerating = false;
        }
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
