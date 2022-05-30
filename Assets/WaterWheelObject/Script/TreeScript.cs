using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : DegenerateBaseScript
{
    //������
    public override void Initialize()
    {
        base.Initialize();
        this.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }

    //�މ�
    public override void Degeneration()
    {
        //�؂̑މ�(�k��)
        this.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
        this.gameObject.transform.localPosition -= new Vector3(0.0f, 0.3f, 1.0f) * Time.deltaTime;

        //�؂̑傫����0.1�����ɂȂ�����މ����~�߂�
        if (this.transform.localScale.y < 0.1f)
        {
            this.gameObject.SetActive(false);
            Degenerating = false;
        }
    }

    //Tree��localScale.y��Getter
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
