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

        [SerializeField]
        Collider mainCollider;
        [SerializeField]
        Collider collisionBlockerCollider;
        
        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;

        [Header("AI Detection Settings")]
        [SerializeField]
        float detectionFOVAngle = 100;
        public float detectionRadius = 20;
        public float currentRecoveryTime = 0;
        public LayerMask detectionLayer;

        [Header("AI Movement Attributes")]
        public float moveSpeedAnimVerticalFloat = 0.75f;
        public float rotationSpeed = 15;
        public float maxAttackRange = 1.5f;

        [Header("Enemy State Machine")]
        public State currentState;
        public CharacterStats currentTarget;
        public bool isPerformingAction;

        public float MinDetectionAngle { get {
                return -(detectionFOVAngle / 2);
            } }
        public float MaxDetectionAngle { get {
                return detectionFOVAngle / 2;
            } }
        public float DistanceFromTarget { get {
                return Vector3.Distance(currentTarget.transform.position, transform.position);
            } }
        public float ViewableAngle { get {
                return Vector3.Angle(currentTarget.transform.position - transform.position, transform.forward);
            } }



        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            rb = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            
        }

        private void Start()
        {
            detectionLayer = (1 << 9);
            navMeshAgent.enabled = false;
            rb.isKinematic = false;

            // Don't let colliders of the same character to bump into each other
            Physics.IgnoreCollision(mainCollider, collisionBlockerCollider, true);
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, animatorManager);

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

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }

}
