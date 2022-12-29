using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponInventorySlotUI : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponHolderSlotManager weaponSlotManager;
        UIManager uIManager;

        public Image icon;

        WeaponItem weapon;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponHolderSlotManager>();
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
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlotInventory[0]);
                playerInventory.weaponsInLeftQuickSlotInventory[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlotInventory[1]);
                playerInventory.weaponsInLeftQuickSlotInventory[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlotInventory[2]);
                playerInventory.weaponsInLeftQuickSlotInventory[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlotInventory[3]);
                playerInventory.weaponsInLeftQuickSlotInventory[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT1)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlotInventory[0]);
                playerInventory.weaponsInRightQuickSlotInventory[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlotInventory[1]);
                playerInventory.weaponsInRightQuickSlotInventory[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlotInventory[2]);
                playerInventory.weaponsInRightQuickSlotInventory[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlotInventory[3]);
                playerInventory.weaponsInRightQuickSlotInventory[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }

            playerInventory.leftWeapon = playerInventory.weaponsInLeftQuickSlotInventory[playerInventory.currentLeftWeaponIndex];
            playerInventory.rightWeapon = playerInventory.weaponsInRightQuickSlotInventory[playerInventory.currentRightWeaponIndex];

            weaponSlotManager.LoadWeaponOnHolderSlot(playerInventory.leftWeapon, true);
            weaponSlotManager.LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);

            uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uIManager.ResetAllSelectedSlots();
        }
    }
}

