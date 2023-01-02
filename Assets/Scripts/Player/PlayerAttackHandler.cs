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
            else if (lastAttack == weapon.ohHeavyAtk1)
            {
                animatorHandler.PlayTargetAnimation(weapon.ohHeavyAtk2, true);
                lastAttack = weapon.ohHeavyAtk2;
            }
            else if (lastAttack == weapon.thLightAtk1)
            {
                animatorHandler.PlayTargetAnimation(weapon.thLightAtk2, true);
                lastAttack = weapon.thLightAtk2;
            }
            else if (lastAttack == weapon.thHeavyAtk1)
            {
                animatorHandler.PlayTargetAnimation(weapon.thHeavyAtk2, true);
                lastAttack = weapon.thHeavyAtk2;
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            latestAttackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.thLightAtk1, true);
                lastAttack = weapon.thLightAtk1;
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
                animatorHandler.PlayTargetAnimation(weapon.thHeavyAtk1, true);
                lastAttack = weapon.thHeavyAtk1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.ohHeavyAtk1, true);
                lastAttack = weapon.ohHeavyAtk1;
            }
        }
    }

}
