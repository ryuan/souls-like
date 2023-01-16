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

        [Header("Critical Attack Attributes")]
        [SerializeField]
        Transform critAtkRaycastStartPoint;

        [SerializeField]
        float backstabDetectionDistance = 0.5f;
        [SerializeField]
        float backstabPosZOffset = -0.75f;
        [SerializeField]
        LayerMask backstabLayer;

        [SerializeField]
        float riposteDetectionDistance = 0.75f;
        [SerializeField]
        float ripostePosZOffset = 1f;
        [SerializeField]
        LayerMask riposteLayer;



        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            animatorManager = GetComponent<PlayerAnimatorManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerInventory = GetComponentInParent<PlayerInventory>();

            backstabLayer = 1 << 12;
            riposteLayer = 1 << 13;
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

        #region Animation Events for Damage Collider

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

        #region Animation Events for Stamina Drain

        public void DrainStaminaLightAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStaminaCost * latestAttackingWeapon.lightAtkStaminaCostMultipler));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(latestAttackingWeapon.baseStaminaCost * latestAttackingWeapon.heavyAtkStaminaCostMultipler));
        }

        #endregion

        #region Animation Events for Parry & Riposte

        public void EnableParrying()
        {
            playerManager.isParrying = true;
        }

        public void DisableParrying()
        {
            playerManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            playerManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            playerManager.canBeRiposted = false;
        }

        #endregion

        #region Animation Events for Spell Casting

        public void CastSpell()
        {
            playerInventory.currentSpell.SpellCastSuccessful(animatorManager, playerStats);
        }

        #endregion

        #region Handle Attack Input Actions

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.weaponType == WeaponType.ShieldWeapon || inputHandler.twoHandFlag)
            {
                HandleLTWeaponArt();
            }
            else if (playerInventory.leftWeapon.weaponType == WeaponType.MeleeWeapon)
            {
                HandleLTMeleeAction();
            }
        }

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

        #endregion

        #region Handle Weapon's Melee/Spell Attack Actions

        private void HandleLTWeaponArt()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            // If two-handing weapon, perform weapon art for RIGHT weapon, else perform weapon art for LEFT weapon
            if (inputHandler.twoHandFlag)
            {
                animatorManager.PlayTargetAnimation(playerInventory.rightWeapon.weaponArt, true);
            }
            else
            {
                animatorManager.PlayTargetAnimation(playerInventory.leftWeapon.weaponArt, true);
            }
        }

        private void HandleLTMeleeAction()
        {
            // Need to build functionality for LEFT hand weapon handling and animations
        }

        private void HandleRBSpellAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }

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

        private void HandleRBMeleeAction()
        {
            if (playerManager.canCombo)
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

        #region Handle Critical Attacks

        public void HandleCriticalAttacks()
        {
            if (playerStats.currentStamina <= 0 || playerManager.isInteracting)
            {
                return;
            }

            RaycastHit hit;

            if (Physics.Raycast(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, backstabDetectionDistance, backstabLayer))
            {
                Debug.DrawRay(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward) * backstabDetectionDistance, Color.yellow, 2f);

                EnemyManager enemyManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyManager != null)
                {
                    // Check for team ID so stop backstab of allies or yourself
                    
                    StartCoroutine(HandleBackstab(hit, enemyManager));
                }
            }
            else if (Physics.Raycast(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, riposteDetectionDistance, riposteLayer))
            {
                Debug.DrawRay(critAtkRaycastStartPoint.position, transform.TransformDirection(Vector3.forward) * riposteDetectionDistance, Color.yellow, 2f);

                EnemyManager enemyManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyManager != null && enemyManager.canBeRiposted)
                {
                    // Check for team ID so stop riposte of allies or yourself
                    
                    StartCoroutine(HandleRiposte(hit, enemyManager));
                }
            }
        }

        IEnumerator HandleBackstab(RaycastHit hit, EnemyManager enemyManager)
        {
            // Slerp to position and look towards target before moving onto backstabbing
            Vector3 targetMovePos = enemyManager.transform.position + enemyManager.transform.forward * backstabPosZOffset;
            Vector3 targetLookPos = hit.transform.position;
            yield return StartCoroutine(playerLocomotion.SlerpFunction(targetMovePos, targetLookPos));

            PrepareForCritAtkAnimationEvents();

            animatorManager.PlayTargetAnimation("Backstab", true);
            enemyManager.GetComponentInChildren<EnemyAnimatorManager>().PlayTargetAnimation("Backstabbed", true);
        }

        IEnumerator HandleRiposte(RaycastHit hit, EnemyManager enemyManager)
        {
            // Slerp to position and look towards target before moving onto riposting
            Vector3 targetMovePos = enemyManager.transform.position + enemyManager.transform.forward * ripostePosZOffset;
            Vector3 targetLookPos = hit.transform.position;
            yield return StartCoroutine(playerLocomotion.SlerpFunction(targetMovePos, targetLookPos));

            PrepareForCritAtkAnimationEvents();

            animatorManager.PlayTargetAnimation("Riposte", true);
            enemyManager.GetComponentInChildren<EnemyAnimatorManager>().PlayTargetAnimation("Riposted", true);
        }

        private void PrepareForCritAtkAnimationEvents()
        {
            // Set crit damage on right weapon Damage Collider
            // Disable default TakeDamage animations to force play riposted animation
            rightWeaponDamageCollider.SetCurrentWeaponDamage(playerInventory.rightWeapon.baseDamage * playerInventory.rightWeapon.critDamageMultiplier);
            rightWeaponDamageCollider.DisableDefaultDamageAnimations();

            // Set latestAttackingWeapon and animator bool for animation events of stamina drain and weapon collider
            latestAttackingWeapon = playerInventory.rightWeapon;
            animatorManager.anim.SetBool("usingRightWeapon", true);
        }

        #endregion
    }
}
