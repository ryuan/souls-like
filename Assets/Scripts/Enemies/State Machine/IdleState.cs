using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class IdleState : State
    {
        [SerializeField]
        PursueTargetState pursueTargetState;



        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, enemyManager.detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // Check for team ID

                    Vector3 targetDir = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDir, transform.forward);

                    if (viewableAngle > enemyManager.MinDetectionAngle && viewableAngle < enemyManager.MaxDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }

            if (enemyManager.currentTarget != null)
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

