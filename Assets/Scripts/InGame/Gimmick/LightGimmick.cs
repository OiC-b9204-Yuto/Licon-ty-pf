using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Gimmick {
    [RequireComponent(typeof(Light))]
    public class LightGimmick : BaseGimmick
    {
        void Start()
        {

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
                if (Active)
                {
                    Active = false;
                }
            }
        }
    }
}