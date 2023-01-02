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
        PlayerAnimatorManager animatorManager;
        Transform mainCameraTransform;
        Animator anim;

        Vector3 moveDirection;
        Vector3 normalVector;
        Vector3 targetPosition;

        public Rigidbody rb;

        [Header("Movement Attributes")]
        [SerializeField]
        float normalMoveSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 80;

        [Header("Grounding & Falling Detection")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        [SerializeField]
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Jump Attributes")]
        [SerializeField]
        float forwardJumpSpeed = 5;
        float jumpStartPosY;



        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            mainCameraTransform = Camera.main.transform;
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        public Vector3 GetMoveDirection()
        {
            return moveDirection;
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
                animatorManager.UpdateAnimatorMovementValues(inputHandler.horizontal, inputHandler.vertical, inputHandler.sprintFlag);
            }
            else
            {
                animatorManager.UpdateAnimatorMovementValues(0, inputHandler.moveAmount, inputHandler.sprintFlag);
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

                    if (inputHandler.twoHandFlag)
                    {
                        animatorManager.PlayTargetAnimation("TH_Rolling", true);
                    }
                    else
                    {
                        animatorManager.PlayTargetAnimation("OH_Rolling", true);
                    }
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        animatorManager.PlayTargetAnimation("TH_Backstep", true);
                    }
                    else
                    {
                        animatorManager.PlayTargetAnimation("OH_Backstep", true);
                    }
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

            if (playerManager.isJumping)
            {
                #region Update grounding/falling state once jump is started
                // Move player forward using velocity
                Vector3 velo = moveDirection;
                velo.Normalize();
                velo *= forwardJumpSpeed;
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(velo, normalVector);
                projectedVelocity.y = rb.velocity.y;    // inhereit y-axis velo from jump animation (set at OnAnimatorMove)
                rb.velocity = projectedVelocity;

                if (anim.deltaPosition.y < 0)
                {
                    Debug.DrawRay(origin, -Vector3.up * groundDetectionRayStartPoint, Color.red, 0.1f, false);
                    if (Physics.Raycast(origin, -Vector3.up, out hit, groundDetectionRayStartPoint, ignoreForGroundCheck))
                    {
                        playerManager.isGrounded = true;
                        playerManager.isFalling = false;
                        playerManager.isJumping = false;
                    }
                    else
                    {
                        playerManager.isGrounded = false;

                        if (transform.position.y <= jumpStartPosY)
                        {
                            playerManager.isFalling = true;
                            playerManager.isJumping = false;
                        }
                        else
                        {
                            playerManager.isFalling = false;
                            playerManager.isJumping = true;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Handle Grounding and Falling
                if (playerManager.isFalling)
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

                    if (playerManager.isFalling)
                    {
                        if (inAirTimer > 0.5f)
                        {
                            animatorManager.PlayTargetAnimation("Land", true);
                            inAirTimer = 0;
                        }
                        else
                        {
                            animatorManager.PlayTargetAnimation("Empty", false);
                            inAirTimer = 0;
                        }

                        playerManager.isFalling = false;
                    }

                    if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                    }
                    else
                    {
                        transform.position = targetPosition;
                    }
                }
                else
                {
                    playerManager.isGrounded = false;

                    if (playerManager.isFalling == false)
                    {
                        playerManager.isFalling = true;

                        if (playerManager.isInteracting == false)
                        {
                            animatorManager.PlayTargetAnimation("Falling", true);
                        }

                        Vector3 vel = rb.velocity;
                        vel.Normalize();
                        rb.velocity = vel * (normalMoveSpeed / 2);
                    }
                }
                #endregion
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (inputHandler.jump_Input && inputHandler.sprintFlag) // jump is only possible when sprinting
            {
                jumpStartPosY = transform.position.y;

                playerManager.isJumping = true;
                playerManager.isGrounded = false;
                playerManager.isFalling = false;
                
                moveDirection = mainCameraTransform.forward * inputHandler.vertical;
                moveDirection += mainCameraTransform.right * inputHandler.horizontal;
                moveDirection.y = 0;

                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = jumpRotation;

                animatorManager.PlayTargetAnimation("Jump", true);
            }
        }
    }
}
