using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public string lastattack;



        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                Debug.Log("Combo activated!");
                if (lastattack == weapon.ohLightAtk1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.ohLightAtk2, true);
                    lastattack = weapon.ohLightAtk2;
                }
                else
                {
                    HandleLightAttack(weapon);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.currentWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.ohLightAtk1, true);
            lastattack = weapon.ohLightAtk1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.currentWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.ohHeavyAtk1, true);
            lastattack = weapon.ohHeavyAtk1;
        }
    }

}
