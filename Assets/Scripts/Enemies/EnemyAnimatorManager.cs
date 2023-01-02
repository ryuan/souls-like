using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyLocomotion enemyLocomotion;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotion = GetComponentInParent<EnemyLocomotion>();
        }

        private void OnAnimatorMove()
        {
            enemyLocomotion.rb.drag = 0;

            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            float delta = Time.deltaTime;
            Vector3 velocity = deltaPosition / delta;
            enemyLocomotion.rb.velocity = velocity;
        }
    }
}
