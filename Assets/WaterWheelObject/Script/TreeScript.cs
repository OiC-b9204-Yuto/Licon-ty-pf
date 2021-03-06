using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : DegenerateBaseScript
{
    //初期化
    public override void Initialize()
    {
        base.Initialize();
        this.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }

    //退化
    public override void Degeneration()
    {
        //木の退化(縮小)
        this.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
        this.gameObject.transform.localPosition -= new Vector3(0.0f, 0.3f, 1.0f) * Time.deltaTime;

        //木の大きさが0.1未満になったら退化を止める
        if (this.transform.localScale.y < 0.1f)
        {
            this.gameObject.SetActive(false);
            Degenerating = false;
        }
    }

    //TreeのlocalScale.yのGetter
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
