using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Licon.Enemy
{
    public interface IStuck
    {
        bool IsStuck();
        void TakeStuck();
    }
}