using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;
        EnemyAnimatorManager animatorManager;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        [SerializeField]
        float detectionFOVAngle = 100;
        public float currentRecoveryTime = 0;

        public bool isPerformingAction;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public float MinDetectionAngle { get { return -detectionFOVAngle / 2; } }
        public float MaxDetectionAngle { get { return detectionFOVAngle / 2; } }



        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotion.currentTarget != null)
            {
                enemyLocomotion.distanceFromTarget = Vector3.Distance(enemyLocomotion.currentTarget.transform.position, transform.position);
            }

            if (enemyLocomotion.currentTarget == null)
            {
                enemyLocomotion.HandleDetection();
            }
            else if (enemyLocomotion.distanceFromTarget > enemyLocomotion.stoppingDistance)
            {
                enemyLocomotion.HandleMoveToTarget();
            }
            else if (enemyLocomotion.distanceFromTarget <= enemyLocomotion.stoppingDistance)
            {
                AttackTarget();
            }
        }

        private void AttackTarget()
        {
            if (isPerformingAction)
            {
                return;
            }

            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                animatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }

        private void GetNewAttack()
        {
            Vector3 targetDir = enemyLocomotion.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDir, transform.forward);
            enemyLocomotion.distanceFromTarget = Vector3.Distance(enemyLocomotion.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.distanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack
                    && enemyLocomotion.distanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack)
                {
                    if (viewableAngle >= enemyAttackAction.minAttackAngle
                        && viewableAngle <= enemyAttackAction.maxAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.distanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack
                    && enemyLocomotion.distanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack)
                {
                    if (viewableAngle >= enemyAttackAction.minAttackAngle
                        && viewableAngle <= enemyAttackAction.maxAttackAngle)
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore >= randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }

}
