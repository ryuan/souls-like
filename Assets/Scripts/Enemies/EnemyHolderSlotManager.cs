using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyHolderSlotManager : MonoBehaviour
    {
        EnemyAttackHandler enemyAttackHandler;

        HolderSlot leftHolderSlot;
        HolderSlot rightHolderSlot;



        private void Awake()
        {
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
            }
            else
            {
                rightHolderSlot.LoadWeaponModel(weapon);
                enemyAttackHandler.SetCurrentWeaponDamageCollider(
                    rightHolderSlot.GetWeaponDamageCollider(), false
                    );
            }
        }
    }
}

