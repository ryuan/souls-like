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
        PlayerLocomotion playerLocomotion;
        PlayerInventory playerInventory;

        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;

        public WeaponItem latestAttackingWeapon;
        public string lastAttack;

        [SerializeField]
        Transform critAtkRaycastStartPoint;
        [SerializeField]
        float critAtkRaycastDistance = 0.5f;
        [SerializeField]
        float backstabPosZOffset = -0.75f;
        [SerializeField]
        LayerMask backstabLayer;



        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            animatorManager = GetComponent<PlayerAnimatorManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerInventory = GetComponentInParent<PlayerInventory>();

            backstabLayer = 1 << 12;
        }

        public void SetCurrentWeaponDamageCollider(DamageCollider damageCollider, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponDamageCollider = damageCollider;
                leftWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.leftWeapon.baseDamage);
            }
            else
            {
                rightWeaponDamageCollider = damageCollider;
                rightWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.rightWeapon.baseDamage);
            }
        }

        #region Handle Weapon's Damage Collider (Animator Events)

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
                leftWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.leftWeapon.baseDamage);
                leftWeaponDamageCollider.DisableDamageCollider();
            }

            if (rightWeaponDamageCollider != null)
            {
                rightWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.rightWeapon.baseDamage);
                rightWeaponDamageCollider.DisableDamageCollider();
            }
        }

        #endregion

        #region Handle Weapon's Stamina Drain (Animator Events)

        public void DrainStaminaLightAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStaminaCost * latestAttackingWeapon.lightAtkStaminaCostMultipler));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStaminaCost * latestAttackingWeapon.heavyAtkStaminaCostMultipler));
        }

        #endregion

        #region Handle Weapon's Melee/Spell Attack Actions

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

        public void CastSpell()
        {
            playerInventory.currentSpell.SpellCastSuccessful(animatorManager, playerStats);
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
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            animatorManager.DisableCombo();

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

        private void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

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

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

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

        #endregion

        public void HandleCriticalAttacks()
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            RaycastHit hit;

            Debug.DrawRay(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward) * critAtkRaycastDistance, Color.red);
            if (Physics.Raycast(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, critAtkRaycastDistance, backstabLayer))
            {
                EnemyManager enemyManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyManager != null)
                {
                    // Check for team ID so stop backstab of allies or yourself
                    
                    StartCoroutine(HandleBackstab(hit, enemyManager));
                }
            }
        }

        IEnumerator HandleBackstab(RaycastHit hit, EnemyManager enemyManager)
        {
            // Slerp to position and look towards target before moving onto backstabbing
            Vector3 targetMovePos = enemyManager.transform.position + enemyManager.transform.forward * backstabPosZOffset;
            Vector3 targetLookPos = hit.transform.position;
            yield return StartCoroutine(playerLocomotion.SlerpFunction(targetMovePos, targetLookPos));

            // Set crit damage on right weapon Damage Collider
            // Disable default TakeDamage animations to force play backstabbed animations
            rightWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.rightWeapon.baseDamage * playerInventory.rightWeapon.critDamageMultiplier);
            rightWeaponDamageCollider.DisableDefaultDamageAnimations();

            // Set latestAttackingWeapon and animator bool for animation events of stamina drain and weapon collider
            latestAttackingWeapon = playerInventory.rightWeapon;
            animatorManager.anim.SetBool("usingRightWeapon", true);

            animatorManager.PlayTargetAnimation("Backstab", true);
            enemyManager.GetComponentInChildren<EnemyAnimatorManager>().PlayTargetAnimation("Backstabbed", true);
        }
    }
}
