using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    [CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void SpellCastAttempted(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
        {
            GameObject instantiatedWarmUpFX = Instantiate(warmUpFX, animatorManager.transform);
            animatorManager.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SpellCastSuccessful(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
        {
            GameObject instantiatedCastFX = Instantiate(castFX, animatorManager.transform);
            playerStats.HealHealth(healAmount);
        }
    }
}

