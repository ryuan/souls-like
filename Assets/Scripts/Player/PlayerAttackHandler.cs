using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerAnimatorManager animatorManager;
        PlayerStats playerStats;
        PlayerManager playerManager;
        PlayerInventory playerInventory;

        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;

        public WeaponItem latestAttackingWeapon;
        public string lastAttack;



        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            animatorManager = GetComponent<PlayerAnimatorManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
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

        public void EnableWeaponDamageCollider()
        {
            if (playerManager.usingLeftWeapon)
            {
                leftWeaponDamageCollider.EnableDamageCollider();
            }

            if (playerManager.usingRightWeapon)
            {
                rightWeaponDamageCollider.EnableDamageCollider();
            }
        }

        public void DisableWeaponDamageCollider()
        {
            if (leftWeaponDamageCollider != null)
            {
                leftWeaponDamageCollider.DisableDamageCollider();
            }

            if (rightWeaponDamageCollider != null)
            {
                rightWeaponDamageCollider.DisableDamageCollider();
            }
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

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.weaponType == WeaponType.FaithCaster || playerInventory.rightWeapon.weaponType == WeaponType.MagicCaster || playerInventory.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                HandleRBSpellAction(playerInventory.rightWeapon);
            }
            else if (playerInventory.rightWeapon.weaponType == WeaponType.MeleeWeapon)
            {
                HandleRBMeleeAction();
            }
        }

        private void HandleRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                animatorManager.anim.SetBool("usingRightWeapon", true);
                HandleWeaponCombo(playerInventory.rightWeapon);
            }
            else
            {
                if (playerManager.isInteracting == false)
                {
                    animatorManager.anim.SetBool("usingRightWeapon", true);
                    HandleLightAttack(playerInventory.rightWeapon);
                }
            }
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            animatorManager.DisableCombo();

            if (playerStats.currentStamina > 0)
            {
                if (lastAttack == weapon.ohLightAtk1)
                {
                    animatorManager.PlayTargetAnimation(weapon.ohLightAtk2, true);
                    lastAttack = weapon.ohLightAtk2;
                }
                else if (lastAttack == weapon.ohHeavyAtk1)
                {
                    animatorManager.PlayTargetAnimation(weapon.ohHeavyAtk2, true);
                    lastAttack = weapon.ohHeavyAtk2;
                }
                else if (lastAttack == weapon.thLightAtk1)
                {
                    animatorManager.PlayTargetAnimation(weapon.thLightAtk2, true);
                    lastAttack = weapon.thLightAtk2;
                }
                else if (lastAttack == weapon.thHeavyAtk1)
                {
                    animatorManager.PlayTargetAnimation(weapon.thHeavyAtk2, true);
                    lastAttack = weapon.thHeavyAtk2;
                }
            }
        }

        private void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina > 0)
            {
                latestAttackingWeapon = weapon;

                if (inputHandler.twoHandFlag)
                {
                    animatorManager.PlayTargetAnimation(weapon.thLightAtk1, true);
                    lastAttack = weapon.thLightAtk1;
                }
                else
                {
                    animatorManager.PlayTargetAnimation(weapon.ohLightAtk1, true);
                    lastAttack = weapon.ohLightAtk1;
                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina > 0)
            {
                latestAttackingWeapon = weapon;

                if (inputHandler.twoHandFlag)
                {
                    animatorManager.PlayTargetAnimation(weapon.thHeavyAtk1, true);
                    lastAttack = weapon.thHeavyAtk1;
                }
                else
                {
                    animatorManager.PlayTargetAnimation(weapon.ohHeavyAtk1, true);
                    lastAttack = weapon.ohHeavyAtk1;
                }
            }
        }

        private void HandleRBSpellAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting == false)
            {
                if (weapon.weaponType == WeaponType.FaithCaster)
                {
                    if (playerInventory.currentSpell != null && playerInventory.currentSpell.spellType == SpellType.Faith)
                    {
                        if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointsCost)
                        {
                            playerInventory.currentSpell.SpellCastAttempted(animatorManager, playerStats);
                        }
                        else
                        {
                            animatorManager.PlayTargetAnimation("Headache", true);
                        }
                    }
                }
            }
        }

        private void CastSpell()
        {
            playerInventory.currentSpell.SpellCastSuccessful(animatorManager, playerStats);
        }
    }
}
