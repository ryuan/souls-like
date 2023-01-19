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
                PlayerLocomotion playerLocomotion = other.GetComponent<PlayerLocomotion>();

                // If parrying, check if this enemy is within a certain angel from player's POV
                // Give more leeway for parrying from the left side than the right since shield is on left hand
                if (playerManager.isParrying && playerLocomotion.IsWithinViewableAngle(animatorManager.transform.position, -60, 45))
                {
                    animatorManager.PlayTargetAnimation("Parried", true);
                    DisableDamageCollider();
                }
                else
                {
                    playerStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }

            if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                EnemyManager enemyManager = other.GetComponent<EnemyManager>();
                EnemyLocomotion enemyLocomotion = other.GetComponent<EnemyLocomotion>();

                // If parrying, check if this player is within a certain angel from enemy's POV
                // Give more leeway for parrying from the left side than the right since shield is on left hand
                if (enemyManager.isParrying && enemyLocomotion.IsWithinViewableAngle(animatorManager.transform.position, -60, 45))
                {
                    animatorManager.PlayTargetAnimation("Parried", true);
                    DisableDamageCollider();
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage, shouldAnimate);
                    shouldAnimate = true;
                }
            }
        }
    }
}

