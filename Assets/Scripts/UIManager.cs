using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class UIManager : MonoBehaviour
    {
        PlayerInventory playerInventory;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentScreenWindow;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("Equipment Window Slot Selected")]
        public Slot selectedSlot = Slot.NONE;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>(true);
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        private void Start()
        {
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            //Debug.Log("weaponInventorySlots.Length: " + weaponInventorySlots.Length);
            //Debug.Log("playerInventory.weaponsInventory.Count: " + playerInventory.weaponsInventory.Count);
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
  
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            selectedSlot = Slot.NONE;
        }
    }
}

