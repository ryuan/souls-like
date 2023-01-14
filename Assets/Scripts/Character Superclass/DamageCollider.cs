using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        float currentWeaponDamage;
        bool shouldAnimate;



        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.enabled = false;
            damageCollider.isTrigger = true;

            shouldAnimate = true;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
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

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }

            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }
        }
    }
}

