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
        NavMeshAgent navMeshAgent;

        LayerMask detectionLayer;

        public Rigidbody rb;
        public CharacterStats currentTarget;

        [Header("Movement Attributes")]
        [SerializeField]
        float moveSpeedAnimVerticalFloat = 0.75f;
        public float stoppingDistance = 0.75f;
        [SerializeField]
        float rotationSpeed = 15;

        public float distanceFromTarget;



        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            detectionLayer = (1 << 9);
            rb.isKinematic = false;
        }

        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // Check for team ID

                    Vector3 targetDir = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDir, transform.forward);

                    if (viewableAngle > enemyManager.MinDetectionAngle && viewableAngle < enemyManager.MaxDetectionAngle)
                    {
                        currentTarget = characterStats;
                    }
                }
            }
        }

        public void HandleMoveToTarget()
        {
            if (enemyManager.isPerformingAction)
            {
                return;
            }

            Vector3 targetDir = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            //float viewableAngle = Vector3.Angle(targetDir, transform.forward);

            // If perfoming any action, stop any movement and navigation (i.e., use the action's root animation movement)
            if (enemyManager.isPerformingAction)
            {
                animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    animatorManager.anim.SetFloat("Vertical", moveSpeedAnimVerticalFloat, 0.1f, Time.deltaTime);

                    HandleFalling();    // temporary function - need to review!
                }
                else
                {
                    animatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }

            HandleRotateTowardsTarget();

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        public void HandleRotateTowardsTarget()
        {
            if (enemyManager.isPerformingAction)    // rotate manually
            {
                Vector3 dir = currentTarget.transform.position - transform.position;
                dir.y = 0;
                dir.Normalize();

                if (dir == Vector3.zero)
                {
                    dir = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            else    // rotate via pathfinding (navmesh)
            {
                Vector3 relativeDir = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = rb.velocity;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                rb.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }

        public void HandleFalling()
        {
            RaycastHit hit;
            Vector3 targetPosition = transform.position;

            Debug.DrawRay(transform.position, -Vector3.up, Color.red, 0.1f, false);
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 2f))
            {
                Vector3 tp = hit.point;
                targetPosition.y = tp.y + 0.2f;

                if (enemyManager.isPerformingAction)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }
    }
}

