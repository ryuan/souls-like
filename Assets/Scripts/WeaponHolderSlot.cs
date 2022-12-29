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

        [SerializeField]
        GameObject currentWeaponModel;



        private void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                return;
            }

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

            currentWeaponModel = model;
        }

        public DamageCollider GetWeaponDamageCollider()
        {
            return currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
    }
}

