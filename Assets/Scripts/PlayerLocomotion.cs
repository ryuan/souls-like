using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerLocomotion : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        Transform mainCameraTransform;

        Vector3 moveDirection;
        Vector3 normalVector;
        Vector3 targetPosition;

        public Rigidbody rb;

        [Header("Movement Stats")]
        [SerializeField]
        float normalMoveSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 80;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        [SerializeField]
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;



        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            mainCameraTransform = Camera.main.transform;
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            animatorHandler.Initialize();
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        public void HandleMovementAndSprint(float delta)
        {
            if (inputHandler.rollFlag || playerManager.isInteracting)
            {
                return;
            }

            moveDirection = mainCameraTransform.forward * inputHandler.vertical;
            moveDirection += mainCameraTransform.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (inputHandler.sprintFlag)
            {
                moveDirection *= sprintSpeed;
            }
            else
            {
                moveDirection *= normalMoveSpeed;
            }

            // Use ProjectOnPlane to detect the surface and keep player "stickied" to the ground
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rb.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.horizontal, inputHandler.vertical, inputHandler.sprintFlag);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(0, inputHandler.moveAmount, inputHandler.sprintFlag);
            }

            HandleRotation(delta);
        }

        public void HandleRotation(float delta)
        {
            Vector3 targetDir;

            if (inputHandler.lockOnFlag == false || inputHandler.sprintFlag || inputHandler.rollFlag)
            {
                targetDir = mainCameraTransform.forward * inputHandler.vertical;
                targetDir += mainCameraTransform.right * inputHandler.horizontal;
                targetDir.y = 0;
                targetDir.Normalize();

                if (targetDir == Vector3.zero)
                {
                    targetDir = transform.forward;
                }
            }
            else
            {
                targetDir = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                targetDir.y = 0;
                targetDir.Normalize();
            }

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);

            transform.rotation = targetRotation;
        }

        public void HandleRoll()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = mainCameraTransform.forward * inputHandler.vertical;
                    moveDirection += mainCameraTransform.right * inputHandler.horizontal;
                    moveDirection.y = 0;
                    
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;

                    animatorHandler.PlayTargetAnimation("Rolling", true);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        public void HandleFalling()
        {
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            Debug.DrawRay(origin, transform.forward * 0.3f, Color.red);
            if (Physics.Raycast(origin, transform.forward, out hit, 0.3f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rb.AddForce(-Vector3.up * fallingSpeed);
                rb.AddForce(moveDirection * fallingSpeed / 7.5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = transform.position;

            Debug.DrawRay(origin, -Vector3.up * minDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                playerManager.isGrounded = true;

                normalVector = hit.normal;
                Vector3 tp = hit.point;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                playerManager.isGrounded = false;

                if (playerManager.isInAir == false)
                {
                    playerManager.isInAir = true;

                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rb.velocity;
                    vel.Normalize();
                    rb.velocity = vel * (normalMoveSpeed / 2);
                }
            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = mainCameraTransform.forward * inputHandler.vertical;
                    moveDirection += mainCameraTransform.right * inputHandler.horizontal;
                    moveDirection.y = 0;

                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = jumpRotation;

                    animatorHandler.PlayTargetAnimation("Jump", true);
                    float jumpHeight = 20f;
                    rb.AddForce(transform.up * Mathf.Sqrt(2 * jumpHeight), ForceMode.VelocityChange);
                }
            }
        }
    }
}
