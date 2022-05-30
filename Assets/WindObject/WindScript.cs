using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick
{
    public class WindScript : BaseGimmick
    {
        AudioSource audiosource;
        private Transform Direction;        //風の向きを取得用
        public Vector3 DirectionWind;       //風の向きをプレイヤーに送る用
                                            // Start is called before the first frame update
        public bool DestroyFlg;             //削除用フラグ
        public float WindTimer;             //風の強さの設定用変数
        private ParticleSystem FX_Wind;

        void Start()
        {
            audiosource = GetComponent<AudioSource>();
            Direction = transform;              //transformを取得
            var directoin = Direction.forward;  //正面の向きを取得
            DirectionWind = directoin;          //プレイヤー用に代入
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
