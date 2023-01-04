using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerStats : CharacterStats
    {
        HealthBar healthBar;
        StaminaBar staminaBar;
        InputHandler inputHandler;
        PlayerAnimatorManager animatorManager;



        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            inputHandler = GetComponent<InputHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
            {
                return;
            }

            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                isDead = true;

                if (inputHandler.twoHandFlag)
                {
                    animatorManager.PlayTargetAnimation("TH_Dead_01", true);
                }
                else
                {
                    animatorManager.PlayTargetAnimation("OH_Dead_01", true);
                }
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    animatorManager.PlayTargetAnimation("TH_Damage_01", true);
                }
                else
                {
                    animatorManager.PlayTargetAnimation("OH_Damage_01", true);
                }
            }
        }

        public void DrainStamina(int cost)
        {
            currentStamina -= cost;
            staminaBar.SetCurrentStamina(currentStamina);
        }
    }
}

