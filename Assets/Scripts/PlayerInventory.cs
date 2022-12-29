using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        [Header("Weapons in Slots")]
        public WeaponItem[] weaponsInLeftQuickSlots = new WeaponItem[4];
        public WeaponItem[] weaponsInRightQuickSlots = new WeaponItem[4];
        public int currentLeftWeaponIndex;
        public int currentRightWeaponIndex;

        public WeaponItem leftWeapon;
        public WeaponItem rightWeapon;
        public WeaponItem unarmed;

        [Header("Weapons in Inventory")]
        public List<WeaponItem> weaponsInventory;



        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            currentLeftWeaponIndex = -1;
            currentRightWeaponIndex = -1;
            ChangeWeapon(true);
            ChangeWeapon(false);
        }

        public void ChangeWeapon(bool isLeft)
        {
            if (isLeft)
            {
                for (int i = currentLeftWeaponIndex + 1; i <= weaponsInLeftQuickSlots.Length; i++)
                {
                    if (weaponsInLeftQuickSlots[i] != null)
                    {
                        currentLeftWeaponIndex = i;
                        leftWeapon = weaponsInLeftQuickSlots[currentLeftWeaponIndex];
                        break;
                    }
                    else if (i == weaponsInLeftQuickSlots.Length)
                    {
                        currentLeftWeaponIndex = -1;
                        leftWeapon = unarmed;
                    }
                }

                weaponSlotManager.LoadWeaponOnHolderSlot(leftWeapon, true);
            }
            else
            {
                for (int i = currentRightWeaponIndex + 1; i <= weaponsInRightQuickSlots.Length; i++)
                {
                    if (weaponsInRightQuickSlots[i] != null)
                    {
                        currentRightWeaponIndex = i;
                        rightWeapon = weaponsInRightQuickSlots[currentRightWeaponIndex];
                        break;
                    }
                    else if (i == weaponsInRightQuickSlots.Length)
                    {
                        currentRightWeaponIndex = -1;
                        rightWeapon = unarmed;
                    }
                }

                weaponSlotManager.LoadWeaponOnHolderSlot(rightWeapon, false);
            }
        }
    }
}

