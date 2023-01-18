using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerHolderSlotManager : MonoBehaviour
    {
        QuickSlotsUI quickSlotsUI;
        Animator animator;
        PlayerAttackHandler playerAttackHandler;
        InputHandler inputHandler;

        HolderSlot leftHolderSlot;
        HolderSlot rightHolderSlot;
        HolderSlot backHolderSlot;
        


        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            animator = GetComponent<Animator>();
            playerAttackHandler = GetComponent<PlayerAttackHandler>();
            inputHandler = GetComponentInParent<InputHandler>();

            HolderSlot[] holderSlots = GetComponentsInChildren<HolderSlot>();

            foreach (HolderSlot holderSlot in holderSlots)
            {
                if (holderSlot.isLeftHandSlot)
                {
                    leftHolderSlot = holderSlot;
                }
                else if (holderSlot.isRightHandSlot)
                {
                    rightHolderSlot = holderSlot;
                }
                else if (holderSlot.isBackSlot)
                {
                    backHolderSlot = holderSlot;
                }
            }
        }

        public void LoadWeaponOnHolderSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                if (inputHandler.twoHandFlag)
                {
                    backHolderSlot.LoadWeaponModel(weaponItem);
                }
                else
                {
                    leftHolderSlot.LoadWeaponModel(weaponItem);
                    playerAttackHandler.SetCurrentWeaponDamageCollider(
                        leftHolderSlot.GetWeaponDamageCollider(), true
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

                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backHolderSlot.LoadWeaponModel(leftHolderSlot.GetWeaponInHolderSlot());
                    leftHolderSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.twoHandIdle, 0.2f);
                }
                else
                {
                    backHolderSlot.UnloadWeaponAndDestroy();

                    #region Handle Right Weapon Idle Animations
                    animator.CrossFade("Both_Arms_Empty", 0.1f);    // force reset animation from two-handed idle state

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

                rightHolderSlot.LoadWeaponModel(weaponItem);
                playerAttackHandler.SetCurrentWeaponDamageCollider(
                    rightHolderSlot.GetWeaponDamageCollider(), false
                    );

                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, false);
            }
        }
    }
}

