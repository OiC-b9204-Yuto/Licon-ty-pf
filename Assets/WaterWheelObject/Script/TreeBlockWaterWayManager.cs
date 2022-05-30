using Licon.Gimmick;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBlockWaterWayManager : BaseGimmick , IAction
{
    //��GameObject
    GameObject Tree;
    //��Script
    TreeScript TreeScript;
    //��GameObject
    GameObject Water;
    //��Script
    SourceWaterScript WaterScript;

    //AudioSource
    protected AudioSource[] AudioSources;
    //�މ�SE
    public AudioClip DegenerateSE;
    //�n����SE
    public AudioClip RumbleSE;
    //��SE
    public AudioClip WaterSE;

    // Start is called before the first frame update
    void Start()
    {
        //�I�u�W�F�N�g�̓ǂݍ���
        Tree = this.gameObject.transform.GetChild(0).gameObject;
        TreeScript = Tree.GetComponent<TreeScript>();
        Water = this.gameObject.transform.GetChild(1).gameObject;
        WaterScript = Water.GetComponent<SourceWaterScript>();

        //AudioSource�̓ǂݍ���
        AudioSources = gameObject.GetComponents<AudioSource>();
        AudioSources[1].clip = WaterSE;

        //�I�u�W�F�N�g�̏�����
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
        //�f�o�b�O�p�R�[�h
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action(null);
        }
        */

        //�؂��މ����̏ꍇ�A�؂�މ�������
        if (TreeScript.GetDegenerating())
        {
            TreeScript.Degeneration();
        }

        //���𓮂���
        //���̑傫���͈��̂Ƃ���Ŏ~�߂�
        if (WaterScript.GetWaterMove())
        {
            WaterScript.UpdateWater();
        }

        //�؂̑މ����I���ɋ߂Â����Ƃ��މ�SE�̍Đ����~�߂�
        if(TreeScript.GetDegenerating() && TreeScript.GetTreeScale() < 0.125f)
        {
            AudioSources[0].Stop();
        }
    }

    //�މ����Z�b�g���A���𓮂����n�߂�
    public void Degenerate()
    {
        TreeScript.SetDegenerating();
        WaterScript.SetWaterMove();
        //�މ�SE�Ɛ�SE�̍Đ�
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
