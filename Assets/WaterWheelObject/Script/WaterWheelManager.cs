using Licon.Gimmick;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWheelManager : BaseGimmick, IAction
{
    //水車GameObject
    protected GameObject Wheel;
    //水車Script
    protected WheelScript WheelScript;
    //つるGameObject
    protected GameObject Vine;
    //つるScript
    protected VineScript VineScript;
    //水GameObject
    protected GameObject Water;
    //水Script
    protected SourceWaterScript WaterScript;

    //AudioSource
    protected AudioSource[] AudioSources;
    //退化SE
    public AudioClip DegenerateSE;
    //水SE
    public AudioClip WaterSE;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //子オブジェクトの読み込み
        Wheel = this.gameObject.transform.GetChild(0).gameObject;
        WheelScript = Wheel.GetComponent<WheelScript>();
        Vine = this.gameObject.transform.GetChild(1).gameObject;
        VineScript = Vine.GetComponent<VineScript>();
        Water = this.gameObject.transform.GetChild(2).gameObject;
        WaterScript = Water.GetComponent<SourceWaterScript>();

        //AudioSourceの読み込み
        AudioSources = gameObject.GetComponents<AudioSource>();
        AudioSources[1].clip = WaterSE;

        //子オブジェクトの初期化
        WheelScript.Initialize();
        VineScript.Initialize();
        WaterScript.Initialize();

        GimickCondition condition = new GimickCondition();
        condition.baseGimmick = this;
        condition.comp = Active;
        conditionList.Add(condition);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //デバッグ用コード
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action(null);
        }
        */

        //つるが退化中の場合、つるを退化させる
        if (VineScript.GetDegenerating())
        {
            VineScript.Degeneration();
        }

        //つるが退化しきった時に、水車を動かすようにするする
        if (!Vine.activeSelf && !WheelScript.GetWheelState())
        {
            WheelScript.SetWheelState();
            //水SEの再生
            AudioSources[1].Play();
        }

        //水車と水を動かす
        //水の大きさは一定のところで止める
        if (WheelScript.GetWheelState())
        {
            WheelScript.UpdateWheel();

            if (WaterScript.GetWaterMove())
            {
                WaterScript.UpdateWater();
            }
        }
    }

    //退化をセットする
    public void Degenerate()
    {
        VineScript.SetDegenerating();
        //退化SEの再生
        AudioSources[0].PlayOneShot(DegenerateSE);
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
