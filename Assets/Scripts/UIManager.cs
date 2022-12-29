using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class UIManager : MonoBehaviour
    {
        PlayerInventory playerInventory;

        [Header("HUD & UI Window GameObjects")]
        [SerializeField]
        GameObject hudWindow;
        [SerializeField]
        GameObject selectWindow;
        [SerializeField]
        GameObject weaponInventoryWindow;
        [SerializeField]
        GameObject equipmentWindow;

        [Header("Weapon Inventory Screen Inputs")]
        [SerializeField]
        GameObject weaponInventorySlotPrefab;
        [SerializeField]
        Transform weaponInventorySlotsParent;
        WeaponInventorySlotUI[] weaponInventoryWindowSlots;

        [Header("Equipment Screen Inputs")]
        public WeaponsQSISlotUI selectedWeaponsQSISlot;
        WeaponsQSISlotUI[] weaponsQSISlots;
        


        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponInventoryWindowSlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlotUI>(true);
        }

        private void Start()
        {
            weaponsQSISlots = GetComponentsInChildren<WeaponsQSISlotUI>(true);
            UpdateEquipmentUI();
        }

        public void UpdateWeaponInventoryUI()
        {
            for (int i = 0; i < weaponInventoryWindowSlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventoryWindowSlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        // Instantiate new Weapon Inventory Slot prefab in Inventory Slot Parent (according to Grid Layout Group component)
                        // Instantiated prefab should be by default inactive based on hierarchy initial state
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventoryWindowSlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlotUI>(true);
                    }
                    weaponInventoryWindowSlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventoryWindowSlots[i].ClearInventorySlot();
                }
            }
        }

        public void UpdateEquipmentUI()
        {
            WeaponItem[] weaponsInLeftQSI = playerInventory.weaponsInLeftQuickSlotInventory;
            WeaponItem[] weaponsInRightQSI = playerInventory.weaponsInRightQuickSlotInventory;

            for (int i = 0; i < weaponsQSISlots.Length; i++)
            {
                if (weaponsQSISlots[i].isLeft)
                {
                    weaponsQSISlots[i].AddItem(weaponsInLeftQSI[weaponsQSISlots[i].slotIndex]);
                }
                else
                {
                    weaponsQSISlots[i].AddItem(weaponsInRightQSI[weaponsQSISlots[i].slotIndex]);
                }
            }
        }

        public void SetActiveHUDWindow(bool isActive)
        {
            hudWindow.SetActive(isActive);
        }

        public void SetActiveSelectWindow(bool isActive)
        {
            selectWindow.SetActive(isActive);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            selectedWeaponsQSISlot = null;
        }
    }
}

