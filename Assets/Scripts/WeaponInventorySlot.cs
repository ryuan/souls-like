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
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlots[0]);
                playerInventory.weaponsInLeftQuickSlots[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlots[1]);
                playerInventory.weaponsInLeftQuickSlots[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlots[2]);
                playerInventory.weaponsInLeftQuickSlots[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.LEFT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftQuickSlots[3]);
                playerInventory.weaponsInLeftQuickSlots[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT1)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlots[0]);
                playerInventory.weaponsInRightQuickSlots[0] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT2)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlots[1]);
                playerInventory.weaponsInRightQuickSlots[1] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT3)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlots[2]);
                playerInventory.weaponsInRightQuickSlots[2] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }
            else if (uIManager.selectedSlot == Slot.RIGHT4)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightQuickSlots[3]);
                playerInventory.weaponsInRightQuickSlots[3] = weapon;
                playerInventory.weaponsInventory.Remove(weapon);
            }

            playerInventory.leftWeapon = playerInventory.weaponsInLeftQuickSlots[playerInventory.currentLeftWeaponIndex];
            playerInventory.rightWeapon = playerInventory.weaponsInRightQuickSlots[playerInventory.currentRightWeaponIndex];

            weaponSlotManager.LoadWeaponOnHolderSlot(playerInventory.leftWeapon, true);
            weaponSlotManager.LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);

            uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uIManager.ResetAllSelectedSlots();
        }
    }
}

