using Licon.Gimmick;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWheelManager : BaseGimmick, IAction
{
    //����GameObject
    protected GameObject Wheel;
    //����Script
    protected WheelScript WheelScript;
    //��GameObject
    protected GameObject Vine;
    //��Script
    protected VineScript VineScript;
    //��GameObject
    protected GameObject Water;
    //��Script
    protected SourceWaterScript WaterScript;

    //AudioSource
    protected AudioSource[] AudioSources;
    //�މ�SE
    public AudioClip DegenerateSE;
    //��SE
    public AudioClip WaterSE;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //�q�I�u�W�F�N�g�̓ǂݍ���
        Wheel = this.gameObject.transform.GetChild(0).gameObject;
        WheelScript = Wheel.GetComponent<WheelScript>();
        Vine = this.gameObject.transform.GetChild(1).gameObject;
        VineScript = Vine.GetComponent<VineScript>();
        Water = this.gameObject.transform.GetChild(2).gameObject;
        WaterScript = Water.GetComponent<SourceWaterScript>();

        //AudioSource�̓ǂݍ���
        AudioSources = gameObject.GetComponents<AudioSource>();
        AudioSources[1].clip = WaterSE;

        //�q�I�u�W�F�N�g�̏�����
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
        //�f�o�b�O�p�R�[�h
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action(null);
        }
        */

        //�邪�މ����̏ꍇ�A���މ�������
        if (VineScript.GetDegenerating())
        {
            VineScript.Degeneration();
        }

        //�邪�މ������������ɁA���Ԃ𓮂����悤�ɂ��邷��
        if (!Vine.activeSelf && !WheelScript.GetWheelState())
        {
            WheelScript.SetWheelState();
            //��SE�̍Đ�
            AudioSources[1].Play();
        }

        //���ԂƐ��𓮂���
        //���̑傫���͈��̂Ƃ���Ŏ~�߂�
        if (WheelScript.GetWheelState())
        {
            WheelScript.UpdateWheel();

            if (WaterScript.GetWaterMove())
            {
                WaterScript.UpdateWater();
            }
        }
    }

    //�މ����Z�b�g����
    public void Degenerate()
    {
        VineScript.SetDegenerating();
        //�މ�SE�̍Đ�
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
