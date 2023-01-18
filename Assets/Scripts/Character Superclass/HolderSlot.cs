using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class HolderSlot : MonoBehaviour
    {
        public Transform handHoldParentOverride;
        public Transform armHoldParentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;

        [SerializeField]
        WeaponItem currentWeapon;
        [SerializeField]
        GameObject currentWeaponModel;



        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            currentWeapon = weaponItem;
            currentWeaponModel = null;

            if (weaponItem.modelPrefab != null)
            {
                GameObject model;

                if (weaponItem.weaponType == WeaponType.ShieldWeapon && armHoldParentOverride != null)
                {
                    model = Instantiate(weaponItem.modelPrefab, armHoldParentOverride);
                }
                else if (handHoldParentOverride != null)
                {
                    model = Instantiate(weaponItem.modelPrefab, handHoldParentOverride);
                }
                else
                {
                    model = Instantiate(weaponItem.modelPrefab, transform);
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;

                currentWeaponModel = model;
            }            
        }

        public WeaponItem GetWeaponInHolderSlot()
        {
            return currentWeapon;
        }

        public DamageCollider GetWeaponDamageCollider()
        {
            return currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
    }
}

