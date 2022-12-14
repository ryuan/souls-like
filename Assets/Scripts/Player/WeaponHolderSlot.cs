using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
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

            GameObject model = Instantiate(weaponItem.modelPrefab);

            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeapon = weaponItem;
            currentWeaponModel = model;
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

