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
        InputHandler inputHandler;

        WeaponHolderSlot leftWeaponHolderSlot;
        WeaponHolderSlot rightWeaponHolderSlot;
        WeaponHolderSlot backWeaponHolderSlot;
        


        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            animator = GetComponent<Animator>();
            playerAttackHandler = GetComponent<PlayerAttackHandler>();
            inputHandler = GetComponentInParent<InputHandler>();

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
                else if (weaponHolderSlot.isBackSlot)
                {
                    backWeaponHolderSlot = weaponHolderSlot;
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
                    leftWeaponHolderSlot.GetWeaponDamageCollider(), true
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
                if (inputHandler.twoHandFlag)
                {
                    backWeaponHolderSlot.LoadWeaponModel(leftWeaponHolderSlot.GetWeaponInHolderSlot());
                    leftWeaponHolderSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.twoHandIdle, 0.2f);
                }
                else
                {
                    backWeaponHolderSlot.UnloadWeaponAndDestroy();

                    #region Handle Right Weapon Idle Animations
                    animator.CrossFade("Both_Arms_Empty", 0.2f);

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

                rightWeaponHolderSlot.LoadWeaponModel(weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, false);
                playerAttackHandler.SetCurrentWeaponDamageCollider(
                    rightWeaponHolderSlot.GetWeaponDamageCollider(), false
                    );
            }
        }
    }
}

