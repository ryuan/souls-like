using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem leftWeapon;
        public WeaponItem rightWeapon;
        public WeaponItem unarmed;

        public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[4];
        public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[4];
        public int currentLeftWeaponIndex;
        public int currentRightWeaponIndex;

        public List<WeaponItem> weaponsInventory;



        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            leftWeapon = unarmed;
            currentLeftWeaponIndex = -1;
            rightWeapon = unarmed;
            currentRightWeaponIndex = -1;

            for (int i = 0; i < weaponsInLeftHandSlot.Length; i++)
            {
                if (weaponsInLeftHandSlot[i] != null)
                {
                    leftWeapon = weaponsInLeftHandSlot[i];
                    currentLeftWeaponIndex = i;
                }

                if (weaponsInRightHandSlot[i] != null)
                {
                    rightWeapon = weaponsInRightHandSlot[i];
                    currentRightWeaponIndex = i;
                }
            }

            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex += 1;

            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] == null)
            {
                currentLeftWeaponIndex += 1;
            }

            if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlot[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlot[1] == null)
            {
                currentLeftWeaponIndex += 1;
            }

            if (currentLeftWeaponIndex == 2 && weaponsInLeftHandSlot[2] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else if (currentLeftWeaponIndex == 2 && weaponsInLeftHandSlot[2] == null)
            {
                currentLeftWeaponIndex += 1;
            }

            if (currentLeftWeaponIndex == 3 && weaponsInLeftHandSlot[3] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else if (currentLeftWeaponIndex == 3 && weaponsInLeftHandSlot[3] == null)
            {
                currentLeftWeaponIndex += 1;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlot.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, true);
            }
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex += 1;

            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlot[0] != null)
            {
                rightWeapon = weaponsInRightHandSlot[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlot[0] == null)
            {
                currentRightWeaponIndex += 1;
            }

            if (currentRightWeaponIndex == 1 && weaponsInRightHandSlot[1] != null)
            {
                rightWeapon = weaponsInRightHandSlot[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlot[1] == null)
            {
                currentRightWeaponIndex += 1;
            }

            if (currentRightWeaponIndex == 2 && weaponsInRightHandSlot[2] != null)
            {
                rightWeapon = weaponsInRightHandSlot[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, true);
            }
            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlot[2] == null)
            {
                currentRightWeaponIndex += 1;
            }

            if (currentRightWeaponIndex == 3 && weaponsInRightHandSlot[3] != null)
            {
                rightWeapon = weaponsInRightHandSlot[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, true);
            }
            else if (currentRightWeaponIndex == 3 && weaponsInRightHandSlot[3] == null)
            {
                currentRightWeaponIndex += 1;
            }
            if (currentRightWeaponIndex > weaponsInRightHandSlot.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, false);
            }
        }
    }
}

