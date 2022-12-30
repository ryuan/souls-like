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
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            latestAttackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.ohLightAtk1, true);
            lastAttack = weapon.ohLightAtk1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            latestAttackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.ohHeavyAtk1, true);
            lastAttack = weapon.ohHeavyAtk1;
        }
    }

}
