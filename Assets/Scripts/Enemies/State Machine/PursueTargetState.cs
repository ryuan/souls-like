using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PursueTargetState : State
    {
        [SerializeField]
        CombatStanceState combatStanceState;



        public override State Tick(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            animatorManager.anim.SetFloat("Vertical", enemyLocomotion.moveSpeedAnimVerticalFloat, 0.1f, Time.deltaTime);
            HandleRotateAndPursue(enemyManager, enemyLocomotion);

            // Pull back navMeshAgent to the unit's main transform since unit now has velocity and rotation
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (enemyLocomotion.DistanceFromTarget <= enemyLocomotion.maxAttackRange)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        public void HandleRotateAndPursue(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion)
        {
            if (enemyManager.isPerformingAction)    // if performing some action, just rotate manually
            {
                enemyLocomotion.HandleRotate();
            }
            else    // otherwise, rotate via nav mesh pathfinding
            {
                Vector3 targetVelocity = enemyManager.rb.velocity;
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.rb.velocity = targetVelocity;

                enemyManager.transform.rotation = Quaternion.Slerp(
                    enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyLocomotion.rotationSpeed * Time.deltaTime
                    );
            }
        }
    }
}
