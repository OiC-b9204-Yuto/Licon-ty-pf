using Licon.Gimmick;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBlockWaterWayManager : BaseGimmick , IAction
{
    //木GameObject
    GameObject Tree;
    //木Script
    TreeScript TreeScript;
    //水GameObject
    GameObject Water;
    //水Script
    SourceWaterScript WaterScript;

    //AudioSource
    protected AudioSource[] AudioSources;
    //退化SE
    public AudioClip DegenerateSE;
    //地響きSE
    public AudioClip RumbleSE;
    //水SE
    public AudioClip WaterSE;

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトの読み込み
        Tree = this.gameObject.transform.GetChild(0).gameObject;
        TreeScript = Tree.GetComponent<TreeScript>();
        Water = this.gameObject.transform.GetChild(1).gameObject;
        WaterScript = Water.GetComponent<SourceWaterScript>();

        //AudioSourceの読み込み
        AudioSources = gameObject.GetComponents<AudioSource>();
        AudioSources[1].clip = WaterSE;

        //オブジェクトの初期化
        WaterScript.Initialize();
        TreeScript.Initialize();
        
        GimickCondition condition = new GimickCondition();
        condition.baseGimmick = this;
        condition.comp = Active;
        conditionList.Add(condition);
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用コード
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action(null);
        }
        */

        //木が退化中の場合、木を退化させる
        if (TreeScript.GetDegenerating())
        {
            TreeScript.Degeneration();
        }

        //水を動かす
        //水の大きさは一定のところで止める
        if (WaterScript.GetWaterMove())
        {
            WaterScript.UpdateWater();
        }

        //木の退化が終了に近づいたとき退化SEの再生を止める
        if(TreeScript.GetDegenerating() && TreeScript.GetTreeScale() < 0.125f)
        {
            AudioSources[0].Stop();
        }
    }

    //退化をセットし、水を動かし始める
    public void Degenerate()
    {
        TreeScript.SetDegenerating();
        WaterScript.SetWaterMove();
        //退化SEと水SEの再生
        AudioSources[0].PlayOneShot(DegenerateSE);
        AudioSources[0].PlayOneShot(RumbleSE);
        AudioSources[1].Play();
    }

    bool isAction = false;
    public void Action(Action endAction)
    {
        if (!isAction)
        {
            Degenerate();
            _endAction = endAction;
            StartCoroutine(WaitActivate(3.0f));
        }
        isAction = true;
    }
    private IEnumerator WaitActivate(float time)
    {
        yield return new WaitForSeconds(time);
        Active = true;
        _endAction?.Invoke();
    }

    public ActionType GetActionType()
    {
        return ConditionCheck() ? ActionType.Degenerate : ActionType.None;
    }
}
