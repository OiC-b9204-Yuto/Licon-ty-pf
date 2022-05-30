    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IStuck
    {
        protected bool stuck = false;

        public bool IsStuck() { return stuck; }

        public abstract void TakeStuck();
    }
}