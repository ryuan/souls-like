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
        AnimatorHandler animatorHandler;



        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
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
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                animatorHandler.PlayTargetAnimation("Dead_01", true);
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    animatorHandler.PlayTargetAnimation("TH_Damage_01", true);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("OH_Damage_01", true);
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

