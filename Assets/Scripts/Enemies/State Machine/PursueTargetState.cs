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
            if (enemyManager.isInteracting)
            {
                enemyLocomotion.HandleRotate();
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
            else
            {
                animatorManager.anim.SetFloat("Vertical", enemyLocomotion.moveSpeedAnimVerticalFloat, 0.1f, Time.deltaTime);
                PursueTarget(enemyManager, enemyLocomotion);
            }

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

        public void PursueTarget(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion)
        {
            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

            enemyManager.transform.rotation = Quaternion.Slerp(
                enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyLocomotion.rotationSpeed * Time.deltaTime
                );
        }
    }
}
