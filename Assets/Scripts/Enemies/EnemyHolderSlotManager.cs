using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyHolderSlotManager : MonoBehaviour
    {
        Animator animator;
        EnemyAttackHandler enemyAttackHandler;

        HolderSlot leftHolderSlot;
        HolderSlot rightHolderSlot;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyAttackHandler = GetComponent<EnemyAttackHandler>();

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
            }
        }

        public void LoadWeaponOnHolderSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHolderSlot.LoadWeaponModel(weapon);
                enemyAttackHandler.SetCurrentWeaponDamageCollider(
                    leftHolderSlot.GetWeaponDamageCollider(), true
                    );

                #region Handle Left Weapon Idle Animations
                if (weapon != null)
                {
                    animator.CrossFade(weapon.leftHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left_Hand_Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                rightHolderSlot.LoadWeaponModel(weapon);
                enemyAttackHandler.SetCurrentWeaponDamageCollider(
                    rightHolderSlot.GetWeaponDamageCollider(), false
                    );

                #region Handle Right Weapon Idle Animations
                if (weapon != null)
                {
                    animator.CrossFade(weapon.rightHandIdle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right_Hand_Empty", 0.2f);
                }
                #endregion
            }
        }
    }
}

