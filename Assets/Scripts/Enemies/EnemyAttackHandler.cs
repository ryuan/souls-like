using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyAttackHandler : MonoBehaviour
    {
        DamageCollider leftWeaponDamageCollider;
        DamageCollider rightWeaponDamageCollider;

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
            rightWeaponDamageCollider.EnableDamageCollider();
        }

        public void DisableWeaponDamageCollider()
        {
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
