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
                return this;
            }

            Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;

            if (enemyManager.DistanceFromTarget > enemyManager.maxAttackRange)
            {
                animatorManager.anim.SetFloat("Vertical", enemyManager.moveSpeedAnimVerticalFloat, 0.1f, Time.deltaTime);

                HandleFalling(enemyManager);    // temporary function - need to review!
            }

            HandleRotateTowardsTarget(enemyManager);

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

        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)    // rotate manually
            {
                Vector3 dir = enemyManager.currentTarget.transform.position - transform.position;
                dir.y = 0;
                dir.Normalize();

                if (dir == Vector3.zero)
                {
                    dir = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else    // rotate via pathfinding (navmesh)
            {
                Vector3 relativeDir = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.rb.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.rb.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        public void HandleFalling(EnemyManager enemyManager)
        {
            RaycastHit hit;
            Vector3 targetPosition = transform.position;

            Debug.DrawRay(transform.position, -Vector3.up, Color.red, 0.1f, false);
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 2f))
            {
                Vector3 tp = hit.point;
                targetPosition.y = tp.y + 0.2f;

                if (enemyManager.isPerformingAction)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }
    }
}
