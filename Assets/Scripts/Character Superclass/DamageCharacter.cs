using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class DamageCharacter : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            CharacterStats characterStats = other.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                if (other.tag == "Player")
                {
                    PlayerStats playerStats = other.GetComponent<PlayerStats>();
                    playerStats.TakeDamage(damage);
                }
                else if (other.tag == "Enemy")
                {
                    EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                    enemyStats.TakeDamage(damage);
                }
            }
        }
    }
}

