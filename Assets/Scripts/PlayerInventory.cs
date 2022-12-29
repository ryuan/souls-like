using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponHolderSlotManager weaponHolderSlotManager;

        [Header("Weapons in Quick Slot Inventory")]
        public WeaponItem[] weaponsInLeftQuickSlotInventory = new WeaponItem[4];
        public WeaponItem[] weaponsInRightQuickSlotInventory = new WeaponItem[4];
        public int currentLeftWeaponIndex;
        public int currentRightWeaponIndex;

        public WeaponItem leftWeapon;
        public WeaponItem rightWeapon;
        public WeaponItem unarmed;

        [Header("Weapons in Inventory")]
        public List<WeaponItem> weaponsInventory;



        private void Awake()
        {
            weaponHolderSlotManager = GetComponentInChildren<WeaponHolderSlotManager>();
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
                for (int i = currentLeftWeaponIndex + 1; i <= weaponsInLeftQuickSlotInventory.Length; i++)
                {
                    if (i == weaponsInLeftQuickSlotInventory.Length)
                    {
                        currentLeftWeaponIndex = -1;
                        leftWeapon = unarmed;
                    }
                    else if (weaponsInLeftQuickSlotInventory[i] != null)
                    {
                        currentLeftWeaponIndex = i;
                        leftWeapon = weaponsInLeftQuickSlotInventory[currentLeftWeaponIndex];
                        break;
                    }
                }

                weaponHolderSlotManager.LoadWeaponOnHolderSlot(leftWeapon, true);
            }
            else
            {
                for (int i = currentRightWeaponIndex + 1; i <= weaponsInRightQuickSlotInventory.Length; i++)
                {
                    if (i == weaponsInRightQuickSlotInventory.Length)
                    {
                        currentRightWeaponIndex = -1;
                        rightWeapon = unarmed;
                    }
                    else if (weaponsInRightQuickSlotInventory[i] != null)
                    {
                        currentRightWeaponIndex = i;
                        rightWeapon = weaponsInRightQuickSlotInventory[currentRightWeaponIndex];
                        break;
                    }
                }

                weaponHolderSlotManager.LoadWeaponOnHolderSlot(rightWeapon, false);
            }
        }
    }
}

