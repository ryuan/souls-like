using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyWeaponHolderSlotManager : MonoBehaviour
    {
        public WeaponItem leftWeapon;
        public WeaponItem rightWeapon;

        EnemyAttackHandler enemyAttackHandler;

        WeaponHolderSlot leftWeaponHolderSlot;
        WeaponHolderSlot rightWeaponHolderSlot;



        private void Awake()
        {
            enemyAttackHandler = GetComponent<EnemyAttackHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftWeaponHolderSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightWeaponHolderSlot = weaponHolderSlot;
                }
            }
        }

        private void Start()
        {
            SetWeaponsInHolderSlots();
        }

        private void SetWeaponsInHolderSlots()
        {
            if (leftWeapon != null)
            {
                LoadWeaponOnHolderSlot(leftWeapon, true);
            }

            if (rightWeapon != null)
            {
                LoadWeaponOnHolderSlot(rightWeapon, false);
            }
        }

        private void LoadWeaponOnHolderSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponHolderSlot.LoadWeaponModel(weapon);
                enemyAttackHandler.SetCurrentWeaponDamageCollider(
                    leftWeaponHolderSlot.GetWeaponDamageCollider(), true
                    );
            }
            else
            {
                rightWeaponHolderSlot.LoadWeaponModel(weapon);
                enemyAttackHandler.SetCurrentWeaponDamageCollider(
                    rightWeaponHolderSlot.GetWeaponDamageCollider(), false
                    );
            }
        }
    }
}

