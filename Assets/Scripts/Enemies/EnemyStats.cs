using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;
        EnemyAttackHandler enemyAttackHandler;



        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            enemyAttackHandler = GetComponentInChildren<EnemyAttackHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private float SetMaxHealthFromHealthLevel()
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
                animator.SetBool("isDead", isDead);

                if (shouldAnimate)
                {
                    animator.Play("OH_Dead_01");
                }
            }
            else
            {
                if (shouldAnimate)
                {
                    animator.Play("OH_Damage_01");
                }
            }

            enemyAttackHandler.DisableWeaponDamageCollider();   // force disable weapon collider if it's open while getting hit
        }
    }
}

