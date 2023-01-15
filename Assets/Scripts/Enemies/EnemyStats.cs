using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager animatorManager;
        EnemyAttackHandler enemyAttackHandler;



        private void Awake()
        {
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyAttackHandler = GetComponentInChildren<EnemyAttackHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(float damage, bool shouldAnimate)
        {
            if (isDead)
            {
                return;
            }

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animatorManager.anim.SetBool("isDead", isDead);

                if (shouldAnimate)
                {
                    animatorManager.PlayTargetAnimation("OH_Dead_01", true);
                }
            }
            else
            {
                if (shouldAnimate)
                {
                    animatorManager.PlayTargetAnimation("OH_Damage_01", true);
                }
            }

            enemyAttackHandler.DisableWeaponDamageCollider();   // force disable weapon collider if it's open while getting hit
        }

        public void AddSouls(int souls)
        {
            soulCount += souls;
        }
    }
}

