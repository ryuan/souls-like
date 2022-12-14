using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(
            EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager
            );
    }
}
