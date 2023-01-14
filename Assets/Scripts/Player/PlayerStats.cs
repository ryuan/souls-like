using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerStats : CharacterStats
    {
        HealthBar healthBar;
        FocusPointsBar focusPointsBar;
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
            focusPointsBar = FindObjectOfType<FocusPointsBar>();
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

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointsBar.SetMaxFocusPoints(maxFocusPoints);

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        private float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(float damage, bool shouldAnimate)
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
                animatorManager.anim.SetBool("isDead", isDead);

                if (shouldAnimate)
                {
                    if (inputHandler.twoHandFlag)
                    {
                        animatorManager.PlayTargetAnimation("TH_Dead_01", true);
                    }
                    else
                    {
                        animatorManager.PlayTargetAnimation("OH_Dead_01", true);
                    }
                }
            }
            else
            {
                if (shouldAnimate)
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

            playerAttackHandler.DisableWeaponDamageCollider();   // force disable weapon collider if it's open while getting hit
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints -= focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
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

        public void HealHealth(int healAmount)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}

