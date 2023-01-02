using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        [SerializeField]
        float detectionFOVAngle = 100;

        bool isPerformingAction;

        public float MinDetectionAngle
        {
            get
            {
                return -detectionFOVAngle / 2;
            }
        }

        public float MaxDetectionAngle
        {
            get
            {
                return detectionFOVAngle / 2;
            }
        }



        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
        }

        private void Update()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotion.currentTarget == null)
            {
                enemyLocomotion.HandleDetection();
            }
        }
    }

}
