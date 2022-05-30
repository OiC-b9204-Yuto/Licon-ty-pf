using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Licon.Gimmick {
    public class ClosedRoomGimmick : BaseGimmick
    {
        void Start()
        {
            ActivateEvents.AddListener(ActivateMessage);
            DeactivateEvents.AddListener(ActivateMessage);
        }

        void Update()
        {
            if (ConditionCheck())
            {
                if (!Active)
                {
                    Active = true;
                }
            }
            else
            {
                //再び閉ざされるなら
                if (Active)
                {
                    Active = false;
                }
            }
        }

        private void ActivateMessage()
        {
            //明るくなる処理
            Debug.Log("空間が開放されました");
        }

        private void DeactivateMessage()
        {
            Debug.Log("空間が閉ざされました");
        }
    }
}