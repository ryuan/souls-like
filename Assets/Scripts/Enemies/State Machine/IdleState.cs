using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class IdleState : State
    {
        [SerializeField]
        PursueTargetState pursueTargetState;

        [SerializeField]
        float detectionRadius = 20;



        public override State Tick(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyLocomotion.detectionLayers);

            for (int i = 0; i < colliders.Length; i++)
            {
                // Get CharacterStats since we may want to let idle enemies attack other enemies, not just player
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // Check for team ID

                    if (enemyLocomotion.IsWithinViewableAngle(characterStats.transform.position, enemyLocomotion.MinDetectionAngle, enemyLocomotion.MaxDetectionAngle))
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

