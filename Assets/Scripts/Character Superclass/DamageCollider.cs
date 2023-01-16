using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class DamageCollider : MonoBehaviour
    {
        Collider weaponCollider;
        AnimatorManager animatorManager;

        float currentWeaponDamage;
        bool shouldAnimate = true;



        private void Awake()
        {
            weaponCollider = GetComponent<Collider>();
            weaponCollider.gameObject.SetActive(true);
            weaponCollider.enabled = false;
            weaponCollider.isTrigger = true;

            animatorManager = GetComponentInParent<AnimatorManager>();
        }

        public void EnableDamageCollider()
        {
            weaponCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            weaponCollider.enabled = false;
        }

        public void SetCurrentWeaponDamage(float damage)
        {
            currentWeaponDamage = damage;
        }

        public void DisableDefaultDamageAnimations()
        {
            shouldAnimate = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                PlayerManager playerManager = other.GetComponent<PlayerManager>();

                if (playerManager != null)
                {
                    if (playerManager.isParrying)
                    {
                        animatorManager.PlayTargetAnimation("Parried", true);
                        return;
                    }
                }

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }

            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                EnemyManager enemyManager = other.GetComponent<EnemyManager>();

                if (enemyManager != null)
                {
                    if (enemyManager.isParrying)
                    {
                        animatorManager.PlayTargetAnimation("Parried", true);
                        return;
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }
        }
    }
}

