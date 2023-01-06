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



        public override State Tick(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            enemyLocomotion.HandleRotate();

            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.currentRecoveryTime <= 0 && enemyLocomotion.DistanceFromTarget <= enemyLocomotion.maxAttackRange && enemyManager.currentTarget.isDead == false)
            {
                return attackState;
            }
            else if (enemyLocomotion.DistanceFromTarget > enemyLocomotion.maxAttackRange)
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
