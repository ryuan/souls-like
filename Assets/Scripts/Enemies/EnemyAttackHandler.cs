using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyAttackHandler : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyWeaponHolderSlotManager weaponHolderSlotManager;

        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;



        private void Awake()
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            weaponHolderSlotManager = GetComponent<EnemyWeaponHolderSlotManager>();
        }

        public void SetCurrentWeaponDamageCollider(DamageCollider damageCollider, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponDamageCollider = damageCollider;
                leftWeaponDamageCollider.SetCurrentWeaponDamage(weaponHolderSlotManager.leftWeapon.baseDamage);
            }
            else
            {
                rightWeaponDamageCollider = damageCollider;
                rightWeaponDamageCollider.SetCurrentWeaponDamage(weaponHolderSlotManager.rightWeapon.baseDamage);
            }
        }

        #region Animation Events for Damage Collider

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

        #region Animation Events for Stamina Drain

        public void DrainStaminaLightAttack()
        {
            // build later
        }

        public void DrainStaminaHeavyAttack()
        {
            // build later
        }

        #endregion

        #region Animation Events for Parry & Riposte

        public void EnableParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        #endregion
    }
}
