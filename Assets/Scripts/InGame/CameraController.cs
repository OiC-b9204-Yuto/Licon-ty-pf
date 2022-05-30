using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Animator targetAnimator;
    public CinemachineVirtualCamera areaCamera;

    private Vector3 offset;

    private GameObject mainCamera;

    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        mainCamera = GameObject.Find("DefaultCamera");
        playerMove = player.GetComponent<PlayerMove>();
    }

    void Update()
    {
        CameraZoom();
        //�_���[�W���󂯂��Ƃ��A���G���Ԃł͂Ȃ��������ʂ�h�炷
        if(targetAnimator.GetBool("Damaged"))
        {
            if (playerMove.isDamage)
            {
                var source = mainCamera.GetComponent<Cinemachine.CinemachineImpulseSource>();
                source.GenerateImpulse();
            }
        }
    }

    void CameraZoom()
    {
        //�A�N�V�����A�j���[�V�������ɃY�[���C���E�Y�[���A�E�g���s��
        if (playerMove.isAction)
        {
            mainCamera.SetActive(false);
        }
        else
        {
            mainCamera.SetActive(true);
        }
        if(playerMove.isZoomArea)
        {
            areaCamera.Priority = 100;
        }
        else
        {
            areaCamera.Priority = 1;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
