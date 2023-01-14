using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public enum WeaponType {
        FaithCaster, MagicCaster, MeleeWeapon, PyroCaster
    };

    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Damage")]
        public int baseDamage;
        public float critDamageMultiplier;

        [Header("Stamina Costs")]
        public int baseStaminaCost;
        public float lightAtkStaminaCostMultipler;
        public float heavyAtkStaminaCostMultipler;

        [Header("Idle Animations")]
        public string leftHandIdle;
        public string rightHandIdle;
        public string twoHandIdle;

        [Header("Attack Animations")]
        public string ohLightAtk1;
        public string ohLightAtk2;
        public string ohHeavyAtk1;
        public string ohHeavyAtk2;
        public string thLightAtk1;
        public string thLightAtk2;
        public string thHeavyAtk1;
        public string thHeavyAtk2;
    }
}
