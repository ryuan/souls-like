using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyLocomotion : MonoBehaviour
    {
        EnemyManager enemyManager;
        LayerMask detectionLayer;

        public CharacterStats currentTarget;



        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            detectionLayer = (1 << 9);
        }

        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

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
                        currentTarget = characterStats;
                    }
                }
            }
        }
    }
}

