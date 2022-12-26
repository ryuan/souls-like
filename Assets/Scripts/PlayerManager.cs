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

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Start()
        {
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
            playerLocomotion.HandleRoll(delta);
            playerLocomotion.HandleJumping(delta);

            CheckForInteractables();
        }

        // FixedUpdate gets called every fixed frames.
        // On frames where it's called, FixedUpdate executes before both Update and LateUpdate!
        // Put any functions that call on Unity's physics engine to affect rigidbodies here.
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            playerLocomotion.HandleMovementAndSprint(delta);
            playerLocomotion.HandleFalling(delta);
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
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        public void CheckForInteractables()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.4f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        string interactableText = interactable.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
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
            Gizmos.DrawWireSphere(transform.position + transform.forward, 0.4f);
        }
    }
}
