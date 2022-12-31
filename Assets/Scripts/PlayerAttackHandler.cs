using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        AnimatorHandler animatorHandler;
        PlayerStats playerStats;

        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;

        public WeaponItem latestAttackingWeapon;
        public string lastAttack;



        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            animatorHandler = GetComponent<AnimatorHandler>();
            playerStats = GetComponentInParent<PlayerStats>();
        }

        public void SetCurrentWeaponDamageCollider(DamageCollider damageCollider, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponDamageCollider = damageCollider;
            }
            else
            {
                rightWeaponDamageCollider = damageCollider;
            }
        }

        #region Handle Weapon's Damage Collider

        public void EnableLeftWeaponDamageCollider()
        {
            leftWeaponDamageCollider.EnableDamageCollider();
        }

        public void EnableRightWeaponDamageCollider()
        {
            rightWeaponDamageCollider.EnableDamageCollider();
        }

        public void DisableLeftWeaponDamageCollider()
        {
            leftWeaponDamageCollider.DisableDamageCollider();
        }

        public void DisableRightWeaponDamageCollider()
        {
            rightWeaponDamageCollider.DisableDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drain
        public void DrainStaminaLightAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStamina * latestAttackingWeapon.lightAtkMultipler));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStamina * latestAttackingWeapon.heavyAtkMultipler));
        }
        #endregion

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            animatorHandler.DisableCombo();

            if (lastAttack == weapon.ohLightAtk1)
            {
                animatorHandler.PlayTargetAnimation(weapon.ohLightAtk2, true);
                lastAttack = weapon.ohLightAtk2;
            }
            else if (lastAttack == weapon.twLightAtk1)
            {
                animatorHandler.PlayTargetAnimation(weapon.twLightAtk2, true);
                lastAttack = weapon.twLightAtk2;
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            latestAttackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.twLightAtk1, true);
                lastAttack = weapon.twLightAtk1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.ohLightAtk1, true);
                lastAttack = weapon.ohLightAtk1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            latestAttackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                // Set up TH heavy attacks
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.ohHeavyAtk1, true);
                lastAttack = weapon.ohHeavyAtk1;
            }
        }
    }

}
