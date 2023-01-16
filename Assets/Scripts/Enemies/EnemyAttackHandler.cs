using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyAttackHandler : MonoBehaviour
    {
        EnemyWeaponHolderSlotManager weaponHolderSlotManager;

        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;



        private void Awake()
        {
            weaponHolderSlotManager = GetComponent<EnemyWeaponHolderSlotManager>();
        }

        public void SetCurrentWeaponDamageCollider(DamageCollider damageCollider, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponDamageCollider = damageCollider;
                leftWeaponDamageCollider.SetCurrentWeaponDamage(weaponHolderSlotManager.leftWeapon.baseDamage);
                leftWeaponDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
            else
            {
                rightWeaponDamageCollider = damageCollider;
                rightWeaponDamageCollider.SetCurrentWeaponDamage(weaponHolderSlotManager.rightWeapon.baseDamage);
                rightWeaponDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
        }

        #region Handle Weapon's Damage Collider

        public void EnableWeaponDamageCollider()
        {
            rightWeaponDamageCollider.EnableDamageCollider();
        }

        public void DisableWeaponDamageCollider()
        {
            rightWeaponDamageCollider.SetCurrentWeaponDamage(weaponHolderSlotManager.rightWeapon.baseDamage);
            rightWeaponDamageCollider.DisableDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drain

        public void DrainStaminaLightAttack()
        {
            // build later
        }

        public void DrainStaminaHeavyAttack()
        {
            // build later
        }

        #endregion
    }
}
