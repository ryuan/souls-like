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
            HandleRotate(enemyManager);

            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.DistanceFromTarget <= enemyManager.maxAttackRange && enemyManager.currentTarget.isDead == false)
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

        private void HandleRotate(EnemyManager enemyManager)
        {
            Vector3 targetDir = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            targetDir.y = 0;
            targetDir.Normalize();

            if (targetDir == Vector3.zero)
            {
                targetDir = enemyManager.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);

            enemyManager.transform.rotation = Quaternion.Slerp(
                enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime
                );
        }
    }
}
