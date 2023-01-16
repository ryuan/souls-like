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
            if (enemyManager.isInteracting)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            enemyLocomotion.HandleRotate();

            if (enemyManager.currentTarget.isDead)
            {
                return this;
            }
            else if (enemyManager.currentRecoveryTime <= 0 && enemyLocomotion.DistanceFromTarget <= enemyLocomotion.maxAttackRange)
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
