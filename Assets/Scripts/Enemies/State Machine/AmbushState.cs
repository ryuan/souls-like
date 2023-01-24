using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class AmbushState : State
    {
        [SerializeField]
        PursueTargetState pursueTargetState;

        [Header("Ambush Animations")]
        [SerializeField]
        string sleepAnimation;
        [SerializeField]
        string wakeAnimation;

        [Header("Detection Settings")]
        [SerializeField]
        bool isSleeping = true;
        [SerializeField]
        float detectionRadius = 4;
        


        public override State Tick(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (isSleeping && enemyManager.isInteracting == false)
            {
                animatorManager.PlayTargetAnimation(sleepAnimation, false);
            }

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyLocomotion.detectionLayers);

            for (int i = 0; i < colliders.Length; i++)
            {
                // Get CharacterStats since we may want to let enemies to ambush other enemies, not just player
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    if (enemyLocomotion.IsWithinViewableAngle(characterStats.transform.position, enemyLocomotion.MinDetectionAngle, enemyLocomotion.MaxDetectionAngle))
                    {
                        Debug.Log(characterStats.gameObject.name);
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        // In case this enemy is woken by being attacked, don't play wake animation
                        if (enemyManager.isInteracting == false)
                        {
                            animatorManager.PlayTargetAnimation(wakeAnimation, true);
                        }
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

