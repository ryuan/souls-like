using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerManager : CharacterManager
    {
        CameraHandler cameraHandler;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        Animator anim;

        [Header("Interactables Attributes")]
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;
        [SerializeField]
        float detectionSphereRadius = 0.5f;
        [SerializeField]
        float detectionSphereHeight = 0.75f;
        [SerializeField]
        float detectionSphereDistance = 0.6f;

        InteractableUI interactableUI;
        Vector3 detectionSphereOffset;

        [Header("Player State Machine")]
        public bool isInteracting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            anim = GetComponentInChildren<Animator>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        // Update is called every frame, before LateUpdate but after FixedUpdate.
        // Put any functions that checks inputs and updates flags here.
        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            anim.applyRootMotion = isInteracting;
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRoll();
            playerLocomotion.HandleJumping();

            CheckForInteractables();
        }

        // FixedUpdate gets called every fixed frames.
        // On frames where it's called, FixedUpdate executes before both Update and LateUpdate!
        // Put any functions that call on Unity's physics engine to affect rigidbodies here.
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            playerLocomotion.HandleMovementAndSprint(delta);
            playerLocomotion.HandleFalling();
            // Need to reset flag in FixedUpdate to match update timing of HandleMovementAndSprint
            inputHandler.sprintFlag = false;
        }

        // LateUpdate is called every frame after all other update functions have finished executing.
        // Put any camera-related functions here so it tracks all objects after they've finished moving.
        // Additionally, any flags/variables that were updated in Update should be reset here.
        private void LateUpdate()
        {
            float delta = Time.fixedDeltaTime;

            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.dPad_Up_Input = false;
            inputHandler.dPad_Down_Input = false;
            inputHandler.dPad_Left_Input = false;
            inputHandler.dPad_Right_Input = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;
            inputHandler.lockOn_Input = false;
            inputHandler.rStick_Left_Input = false;
            inputHandler.rStick_Right_Input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        public void CheckForInteractables()
        {
            detectionSphereOffset = new Vector3(0, detectionSphereHeight) + (transform.forward * detectionSphereDistance);
            Vector3 checkPosition = transform.position + detectionSphereOffset;
            Collider[] hitColliders = Physics.OverlapSphere(
                checkPosition, detectionSphereRadius, cameraHandler.ignoreLayers, QueryTriggerInteraction.Collide
                );

            if (hitColliders.Length > 0)
            {
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "Interactable")
                    {
                        Interactable interactable = hitCollider.GetComponent<Interactable>();

                        if (interactable != null)
                        {
                            string interactableText = interactable.interactableText;
                            interactableUI.interactableText.text = interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (inputHandler.a_Input)
                            {
                                hitCollider.GetComponent<Interactable>().Interact();
                            }
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                }
            }
        }

        private void OnDrawGizmos()
        {
            // For debugging CheckForInteractables function and its SphereCast collision against interactable colliders
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + detectionSphereOffset, detectionSphereRadius);
        }
    }
}
