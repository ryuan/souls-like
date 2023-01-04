using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class AttackState : State
    {
        [SerializeField]
        CombatStanceState combatStanceState;

        [SerializeField]
        EnemyAttackAction[] enemyAttacks;
        [SerializeField]
        EnemyAttackAction currentAttack;



        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            HandleRotate(enemyManager);

            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }

            if (currentAttack != null)
            {
                // If the target is within the attack's viable angle, then proceed
                if (enemyManager.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, currentAttack.MinAttackAngle, currentAttack.MaxAttackAngle))
                {
                    // If this unit is too close to the target, then try again
                    // This case shouldn't happen if at least one attack has min distance of 0
                    if (enemyManager.DistanceFromTarget < currentAttack.minAttackDistance)
                    {
                        return this;
                    }
                    // If this unit is within range to attack the target, then proceed
                    else if (enemyManager.DistanceFromTarget < currentAttack.maxAttackDistance)
                    {
                        // If the past action's recovery time has expired, then attack and reset
                        if (enemyManager.currentRecoveryTime <= 0)
                        {
                            animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            animatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            animatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);

                            enemyManager.isPerformingAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

                            currentAttack = null;

                            return combatStanceState;
                        }
                    }
                }
            }

            // If you unit didn't execute an attack, that means either (1) currentAttack is empty or (2) currentAttack is out of range
            GetNewAttack(enemyManager);
            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.DistanceFromTarget >= enemyAttackAction.minAttackDistance
                    && enemyManager.DistanceFromTarget <= enemyAttackAction.maxAttackDistance)
                {
                    if (enemyManager.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, enemyAttackAction.MinAttackAngle, enemyAttackAction.MaxAttackAngle))
                    {
                        maxScore += enemyAttackAction.attackChanceWeight;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore + 1);  // need +1 since Range function excludes max value when it is an int
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.DistanceFromTarget >= enemyAttackAction.minAttackDistance
                    && enemyManager.DistanceFromTarget <= enemyAttackAction.maxAttackDistance)
                {
                    if (enemyManager.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, enemyAttackAction.MinAttackAngle, enemyAttackAction.MaxAttackAngle))
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }

                        tempScore += enemyAttackAction.attackChanceWeight;

                        if (tempScore >= randomValue)
                        {
                            currentAttack = enemyAttackAction;

                        }
                    }
                }
            }
        }

        private void HandleRotate(EnemyManager enemyManager)
        {
            Vector3 targetDir = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            targetDir.y = 0;
            targetDir.Normalize();

            if (targetDir == Vector3.zero)
            {
                targetDir = enemyManager.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);

            enemyManager.transform.rotation = Quaternion.Slerp(
                enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime
                );
        }
    }
}
