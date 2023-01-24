using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RY
{
    public class EnemyManager : CharacterManager
    {
        EnemyStats enemyStats;
        EnemyAnimatorManager animatorManager;
        EnemyLocomotion enemyLocomotion;
        
        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;

        [Header("State Machine Attributes")]
        public State initialState;
        public State currentState;
        public CharacterStats currentTarget;
        public float currentRecoveryTime = 0;

        [Header("State Conditions")]
        public bool isInteracting;
        public bool canRotate;
        public bool canCombo;



        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            rb = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Start()
        {
            currentState = initialState;

            navMeshAgent.enabled = false;
            rb.isKinematic = false;

            // Don't let colliders of the same character to bump into each other
            Physics.IgnoreCollision(mainCollider, collisionBlockerCollider, true);
        }

        private void Update()
        {
            UpdateConditions();
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            UpdateConditions();
            HandleStateMachine();
            enemyLocomotion.HandleGrounding();
        }

        private void UpdateConditions()
        {
            isInteracting = animatorManager.anim.GetBool("isInteracting");
            canRotate = animatorManager.anim.GetBool("canRotate");
            canCombo = animatorManager.anim.GetBool("canCombo");
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyLocomotion, enemyStats, animatorManager);

                if (nextState != null)
                {
                    // Set new state for next FixedUpdate's handling of state machine
                    currentState = nextState;
                }
            }
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }
        }
    }
}
