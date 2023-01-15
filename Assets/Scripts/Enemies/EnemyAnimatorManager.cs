using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        PlayerStats playerStats;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
            playerStats = FindObjectOfType<PlayerStats>();
        }

        public void EnableRotation()
        {
            //anim.SetBool("canRotate", true);
        }

        public void DisableRotation()
        {
            //anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }

        public void AwardSoulsOnDeath()
        {
            playerStats.AddSouls(enemyStats.soulCount);
        }

        private void OnAnimatorMove()
        {
            enemyManager.rb.drag = 0;

            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            float delta = Time.deltaTime;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.rb.velocity = velocity;
        }
    }
}
