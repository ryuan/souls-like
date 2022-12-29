using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class UIManager : MonoBehaviour
    {
        PlayerInventory playerInventory;
        [SerializeField]
        GameObject hudWindow;

        [Header("UI Window GameObjects")]
        [SerializeField]
        GameObject selectWindow;
        [SerializeField]
        GameObject weaponInventoryWindow;
        [SerializeField]
        GameObject equipmentWindow;

        [Header("UI Window Scripts")]
        public EquipmentWindowUI equipmentWindowUI;

        [Header("Equipment Window Slot Selected")]
        public Slot selectedSlot = Slot.NONE;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlotUI[] weaponInventoryWindowSlots;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();

            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>(true);
            weaponInventoryWindowSlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlotUI>(true);
        }

        private void Start()
        {
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            #region Update the slots in Weapon Inventory Window

            for (int i = 0; i < weaponInventoryWindowSlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventoryWindowSlots.Length < playerInventory.weaponsInventory.Count)
                    {
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

            #endregion
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
            selectedSlot = Slot.NONE;
        }
    }
}

