using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class AttackState : State
    {
        [SerializeField]
        CombatStanceState combatStanceState;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }

            if (currentAttack != null)
            {
                // If this unit is too close to the target, then try again
                if (enemyManager.DistanceFromTarget < currentAttack.minDistanceNeededToAttack)
                {
                    return this;
                }
                // If this unit is within range to attack the target, then proceed
                else if (enemyManager.DistanceFromTarget < currentAttack.maxDistanceNeededToAttack)
                {
                    // If the target is within the attack's viable angle, then proceed
                    if (enemyManager.ViewableAngle <= currentAttack.maxAttackAngle
                        && enemyManager.ViewableAngle >= currentAttack.minAttackAngle)
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
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.DistanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack
                    && enemyManager.DistanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack)
                {
                    if (enemyManager.ViewableAngle >= enemyAttackAction.minAttackAngle
                        && enemyManager.ViewableAngle <= enemyAttackAction.maxAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore + 1);  // need +1 since Range function excludes max value when it is an int
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.DistanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack
                    && enemyManager.DistanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack)
                {
                    if (enemyManager.ViewableAngle >= enemyAttackAction.minAttackAngle
                        && enemyManager.ViewableAngle <= enemyAttackAction.maxAttackAngle)
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
    }
}
