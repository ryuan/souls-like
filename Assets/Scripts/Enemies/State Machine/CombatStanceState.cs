using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class CombatStanceState : State
    {
        [SerializeField]
        AttackState attackState;
        [SerializeField]
        PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.DistanceFromTarget <= enemyManager.maxAttackRange)
            {
                return attackState;
            }
            else if (enemyManager.DistanceFromTarget > enemyManager.maxAttackRange)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}
