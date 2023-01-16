using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponInventorySlotUI : MonoBehaviour
    {
        PlayerInventory playerInventory;
        PlayerHolderSlotManager holderSlotManager;
        UIManager uIManager;

        [SerializeField]
        WeaponItem weapon;
        [SerializeField]
        Image icon;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            holderSlotManager = FindObjectOfType<PlayerHolderSlotManager>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
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

        public void AddThisWeaponToQSI()
        {
            WeaponsQSISlotUI selectedSlot = uIManager.selectedWeaponsQSISlot;

            if (selectedSlot != null)
            {
                if (selectedSlot.isLeft)
                {
                    // Add the selected weapon from the Quick Slot Inventory (if it's not empty) to the Weapon Inventory
                    if (playerInventory.weaponsInLeftQuickSlotInventory[selectedSlot.slotIndex] != null)
                    {
                        playerInventory.weaponsInventory.Add(
                        playerInventory.weaponsInLeftQuickSlotInventory[selectedSlot.slotIndex]
                        );
                    }
                    // Overwrite the selected Quick Slot Inventory weapon with the weapon from this Weapon Inventory slot
                    playerInventory.weaponsInLeftQuickSlotInventory[selectedSlot.slotIndex] = weapon;
                    // Remove this weapon from the Weapon Inventory (future update calls to Weapon Inventory will deactivate this slot)
                    playerInventory.weaponsInventory.Remove(weapon);

                    // Force refresh the active left weapon in case the new weapon is in the current QS index
                    if (playerInventory.currentLeftWeaponIndex == selectedSlot.slotIndex)
                    {
                        playerInventory.leftWeapon = playerInventory.weaponsInLeftQuickSlotInventory[playerInventory.currentLeftWeaponIndex];
                        holderSlotManager.LoadWeaponOnHolderSlot(playerInventory.leftWeapon, true);
                    }
                }
                else
                {
                    if (playerInventory.weaponsInRightQuickSlotInventory[selectedSlot.slotIndex] != null)
                    {
                        playerInventory.weaponsInventory.Add(
                            playerInventory.weaponsInRightQuickSlotInventory[selectedSlot.slotIndex]
                            );
                    }
                    playerInventory.weaponsInRightQuickSlotInventory[selectedSlot.slotIndex] = weapon;
                    playerInventory.weaponsInventory.Remove(weapon);

                    if (playerInventory.currentRightWeaponIndex == selectedSlot.slotIndex)
                    {
                        playerInventory.rightWeapon = playerInventory.weaponsInRightQuickSlotInventory[playerInventory.currentRightWeaponIndex];
                        holderSlotManager.LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);
                    }
                }

                uIManager.UpdateEquipmentUI();
                uIManager.CloseAllOpenMenuWindows();
            }
        }
    }
}

