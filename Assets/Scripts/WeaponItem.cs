using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string leftHandIdle;
        public string rightHandIdle;

        [Header("Attack Animations")]
        public string ohLightAtk1;
        public string ohLightAtk2;
        public string ohHeavyAtk1;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAtkMultipler;
        public float heavyAtkMultipler;
    }
}
