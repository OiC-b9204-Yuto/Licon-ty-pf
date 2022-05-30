using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    public class WindScript : BaseGimmick
    {
        AudioSource audiosource;
        private Transform Direction;        //���̌������擾�p
        public Vector3 DirectionWind;       //���̌������v���C���[�ɑ���p
                                            // Start is called before the first frame update
        public bool DestroyFlg;             //�폜�p�t���O
        public float WindTimer;             //���̋����̐ݒ�p�ϐ�
        private ParticleSystem FX_Wind;

        void Start()
        {
            audiosource = GetComponent<AudioSource>();
            Direction = transform;              //transform���擾
            var directoin = Direction.forward;  //���ʂ̌������擾
            DirectionWind = directoin;          //�v���C���[�p�ɑ��
            Transform fxwind = GameObject.Find("FX_WIND").transform;
            if(fxwind)
            {
                FX_Wind = fxwind.GetComponent<ParticleSystem>();
            }
            DestroyFlg = false;
        }
        private void Update()
        {
            if (ConditionCheck())
            {
                if (DestroyFlg)
                {
                    FX_Wind.Stop();
                    transform.position -= new Vector3(0,50,0);
                    //gameObject.SetActive(false);
                    //Destroy(this.gameObject);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                audiosource.Play();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                audiosource.Stop();
            }
        }
    }
}
