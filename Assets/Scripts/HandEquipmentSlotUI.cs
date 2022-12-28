using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Slot handSlot;
        public Image icon;

        [SerializeField]
        WeaponItem weapon;



        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            if (newWeapon != null)
            {
                weapon = newWeapon;
                icon.sprite = weapon.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.selectedSlot = handSlot;
        }
    }

}
