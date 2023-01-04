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
        


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (isSleeping && animatorManager.anim.GetBool("isInteracting") == false)
            {
                animatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyManager.detectionLayers);

            for (int i = 0; i < colliders.Length; i++)
            {
                // Get CharacterStats since we may want to let enemies to ambush other enemies, not just player
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    if (enemyManager.IsWithinViewableAngle(characterStats.transform.position, enemyManager.MinDetectionAngle, enemyManager.MaxDetectionAngle))
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        animatorManager.PlayTargetAnimation(wakeAnimation, true);
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

