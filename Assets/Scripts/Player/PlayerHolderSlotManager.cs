using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerHolderSlotManager : MonoBehaviour
    {
        QuickSlotsUI quickSlotsUI;
        Animator animator;
        PlayerInventory playerInventory;
        PlayerAttackHandler playerAttackHandler;
        InputHandler inputHandler;

        HolderSlot leftHolderSlot;
        HolderSlot rightHolderSlot;
        HolderSlot backHolderSlot;
        


        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            animator = GetComponent<Animator>();
            playerInventory = GetComponentInParent<PlayerInventory>();
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
                        animator.CrossFade("Left_Hand_Empty", 0.2f);
                    }
                    #endregion
                }

                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    if (backHolderSlot.GetWeaponInHolderSlot() == null)
                    {
                        backHolderSlot.LoadWeaponModel(leftHolderSlot.GetWeaponInHolderSlot());
                        leftHolderSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.twoHandIdle, 0.2f);
                    }
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
                        animator.CrossFade("Right_Hand_Empty", 0.2f);
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

        #region Handle One/Two Handing Weapon Swap

        public void HandleTwoHanding()
        {
            if (playerInventory.leftWeapon.isUnarmed)
            {
                UpdateTwoHanding();
            }
            else
            {
                animator.CrossFade(playerInventory.leftWeapon.ohLeftHandSwap, 0.2f);
            }
        }

        public void UpdateTwoHanding()
        {
            inputHandler.twoHandFlag = !inputHandler.twoHandFlag;

            if (inputHandler.twoHandFlag)
            {
                LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                LoadWeaponOnHolderSlot(playerInventory.leftWeapon, true);
                LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);
            }
        }


        // Animation event for two hand swapping
        public void SwapBetweenOneAndTwoHanding()
        {
            UpdateTwoHanding();
        }

        #endregion
    }
}