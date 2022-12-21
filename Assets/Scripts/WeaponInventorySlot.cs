using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        public Image icon;

        WeaponItem weapon;

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
    }
}

