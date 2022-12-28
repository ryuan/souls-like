using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uIManager;

        public Image icon;

        WeaponItem weapon;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            weapon = newItem;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uIManager.selectedSlot == Slot.LEFT1)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[0]);
                playerInventory.weaponsInLeftHandSlot[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[1]);
                playerInventory.weaponsInLeftHandSlot[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[2]);
                playerInventory.weaponsInLeftHandSlot[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[3]);
                playerInventory.weaponsInLeftHandSlot[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT1)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[0]);
                playerInventory.weaponsInRightHandSlot[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[1]);
                playerInventory.weaponsInRightHandSlot[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[2]);
                playerInventory.weaponsInRightHandSlot[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[3]);
                playerInventory.weaponsInRightHandSlot[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }

            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlot[playerInventory.currentLeftWeaponIndex];
            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlot[playerInventory.currentRightWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);

            uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uIManager.ResetAllSelectedSlots();
        }
    }
}

