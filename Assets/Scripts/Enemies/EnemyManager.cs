using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RY
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;
        EnemyStats enemyStats;
        EnemyAnimatorManager animatorManager;

        public State currentState;
        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;

        public CharacterStats currentTarget;

        [Header("AI Detection Settings")]
        public float detectionRadius = 20;
        [SerializeField]
        float detectionFOVAngle = 100;
        public float currentRecoveryTime = 0;
        public LayerMask detectionLayer;

        [Header("AI Movement Attributes")]
        public float moveSpeedAnimVerticalFloat = 0.75f;
        public float rotationSpeed = 15;
        public float maxAttackRange = 1.5f;

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
        public float ViewableAngle { get
            {
                return Vector3.Angle(currentTarget.transform.position - transform.position, transform.forward);
            } }



        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyStats = GetComponent<EnemyStats>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            detectionLayer = (1 << 9);
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
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
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State nextState)
        {
            currentState = nextState;
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
