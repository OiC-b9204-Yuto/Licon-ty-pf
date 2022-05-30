using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Enemy;

namespace Licon.Gimmick
{
    public class EnemyStuckChecker : BaseGimmick
    {
        public List<EnemyBase> EnemyList = new List<EnemyBase>();

        void Update()
        {
            if (!Active)
            {
                if (EnemyCheck())
                {
                    Active = true;
                }
            }
        }

        bool EnemyCheck()
        {
            foreach (var item in EnemyList)
            {
                if (item.IsStuck() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}