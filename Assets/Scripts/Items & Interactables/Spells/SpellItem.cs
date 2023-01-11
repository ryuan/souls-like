using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public enum SpellType {
        Faith, Magic, Pyro
    };

    public class SpellItem : Item
    {
        [Header("Spell VFX")]
        public GameObject warmUpFX;
        public GameObject castFX;

        [Header("Character Cast Animation")]
        public string spellAnimation;

        [Header("Spell Type")]
        public SpellType spellType;

        [Header("Focus Points Cost")]
        public int focusPointsCost;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;



        public virtual void SpellCastAttempted(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
        {
            Debug.Log("You attempted to cast a spell");
        }

        public virtual void SpellCastSuccessful(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
        {
            Debug.Log("You successfully cast a spell");

            playerStats.DeductFocusPoints(focusPointsCost);
        }
    }
}
