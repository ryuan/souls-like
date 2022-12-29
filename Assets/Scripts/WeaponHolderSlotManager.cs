using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class WeaponHolderSlotManager : MonoBehaviour
    {
        QuickSlotsUI quickSlotsUI;
        Animator animator;
        PlayerAttackHandler playerAttackHandler;

        WeaponHolderSlot leftWeaponHolderSlot;
        WeaponHolderSlot rightWeaponHolderSlot;
        


        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            animator = GetComponent<Animator>();
            playerAttackHandler = GetComponent<PlayerAttackHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftWeaponHolderSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightWeaponHolderSlot = weaponHolderSlot;
                }
            }
        }

        public void LoadWeaponOnHolderSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftWeaponHolderSlot.LoadWeaponModel(weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
                playerAttackHandler.SetCurrentWeaponDamageCollider(
                    leftWeaponHolderSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>(), true
                    );

                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.leftHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left_Arm_Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                rightWeaponHolderSlot.LoadWeaponModel(weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, false);
                playerAttackHandler.SetCurrentWeaponDamageCollider(
                    rightWeaponHolderSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>(), false
                    );

                #region Handle Right Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.rightHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right_Arm_Empty", 0.2f);
                }
                #endregion
            }
        }
    }
}

