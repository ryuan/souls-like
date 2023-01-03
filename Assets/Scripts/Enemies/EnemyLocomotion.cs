using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RY
{
    public class EnemyLocomotion : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager animatorManager;

        

        
        

        

        public float distanceFromTarget;



        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            
        }

        


    }
}

