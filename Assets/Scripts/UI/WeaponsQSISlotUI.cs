using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponsQSISlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public bool isLeft;
        public int slotIndex;

        [SerializeField]
        WeaponItem weapon;
        [SerializeField]
        Image icon;



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
            uIManager.selectedWeaponsQSISlot = this;
        }
    }

}
