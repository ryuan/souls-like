using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public float currentHealth;

        public int focusLevel = 10;
        public int maxFocusPoints;
        public float currentFocusPoints;

        public int staminaLevel = 10;
        public int maxStamina;
        public float currentStamina;

        public int soulCount = 0;

        public bool isDead;
    }
}