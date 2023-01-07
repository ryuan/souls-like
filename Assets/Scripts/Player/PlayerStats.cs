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
        PlayerManager playerManager;
        PlayerAttackHandler playerAttackHandler;

        [SerializeField]
        float staminaRegenRate = 1;
        [SerializeField]
        float staminaRegenTimer = 0;
        [SerializeField]
        float staminaRegenLockTime = 1.5f;



        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            inputHandler = GetComponent<InputHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            playerAttackHandler = GetComponentInChildren<PlayerAttackHandler>();
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

        private float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if (isDead || playerManager.isInvulnerable)
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

            playerAttackHandler.DisableWeaponDamageCollider();   // force disable weapon collider if it's open while getting hit
        }

        public void DrainStamina(int cost)
        {
            currentStamina -= cost;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > staminaRegenLockTime)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }
    }
}

