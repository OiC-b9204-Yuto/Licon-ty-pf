using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineScript : DegenerateBaseScript
{

    public override void Degeneration()
    {
        //つるの退化
        this.gameObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;

        //つるの大きさが0.1未満になったら退化を止める
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
