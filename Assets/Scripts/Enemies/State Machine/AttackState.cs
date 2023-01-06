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



        public override State Tick(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion, EnemyStats enemyStats, EnemyAnimatorManager animatorManager)
        {
            enemyLocomotion.HandleRotate();

            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }

            if (currentAttack != null)
            {
                // If the target is within the attack's viable angle, then proceed
                if (enemyLocomotion.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, currentAttack.MinAttackAngle, currentAttack.MaxAttackAngle))
                {
                    // If this unit is too close to the target, then try again
                    // This case shouldn't happen if at least one attack has min distance of 0
                    if (enemyLocomotion.DistanceFromTarget < currentAttack.minAttackDistance)
                    {
                        return this;
                    }
                    // If this unit is within range to attack the target, then proceed
                    else if (enemyLocomotion.DistanceFromTarget < currentAttack.maxAttackDistance)
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
            GetNewAttack(enemyManager, enemyLocomotion);
            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager, EnemyLocomotion enemyLocomotion)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.DistanceFromTarget >= enemyAttackAction.minAttackDistance
                    && enemyLocomotion.DistanceFromTarget <= enemyAttackAction.maxAttackDistance)
                {
                    if (enemyLocomotion.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, enemyAttackAction.MinAttackAngle, enemyAttackAction.MaxAttackAngle))
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

                if (enemyLocomotion.DistanceFromTarget >= enemyAttackAction.minAttackDistance
                    && enemyLocomotion.DistanceFromTarget <= enemyAttackAction.maxAttackDistance)
                {
                    if (enemyLocomotion.IsWithinViewableAngle(enemyManager.currentTarget.transform.position, enemyAttackAction.MinAttackAngle, enemyAttackAction.MaxAttackAngle))
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
    }
}
