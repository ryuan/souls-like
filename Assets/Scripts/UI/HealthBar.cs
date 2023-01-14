using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;



        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealth(float currentHealth)
        {
            slider.value = currentHealth;
        }
    }

}
