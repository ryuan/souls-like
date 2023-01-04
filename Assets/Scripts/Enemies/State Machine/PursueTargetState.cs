using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PursueTargetState : State
    {
        [SerializeField]
        CombatStanceState combatStanceState;



        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            animatorManager.anim.SetFloat("Vertical", enemyManager.moveSpeedAnimVerticalFloat, 0.1f, Time.deltaTime);
            HandleRotateAndPursue(enemyManager);

            // Pull back navMeshAgent to the unit's main transform since unit now has velocity and rotation
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (enemyManager.DistanceFromTarget <= enemyManager.maxAttackRange)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        public void HandleRotateAndPursue(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)    // rotate manually
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
            else    // rotate via nav mesh pathfinding
            {
                Vector3 targetVelocity = enemyManager.rb.velocity;
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.rb.velocity = targetVelocity;

                enemyManager.transform.rotation = Quaternion.Slerp(
                    enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime
                    );
            }
        }
    }
}
