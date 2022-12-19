using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        Animator anim;

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
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            anim.applyRootMotion = isInteracting;
            canDoCombo = anim.GetBool("canDoCombo");

            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollAndSprint(delta);
            playerLocomotion.HandleFalling(delta);

            CheckForInteractables();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            isSprinting = inputHandler.sprintFlag;
            inputHandler.dPad_Up_Input = false;
            inputHandler.dPad_Down_Input = false;
            inputHandler.dPad_Left_Input = false;
            inputHandler.dPad_Right_Input = false;
            inputHandler.a_Input = false;

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
                        string interactableTest = interactable.interactableText;
                        // Set the UI Text to Interactable's text
                        // Set the text pop up to true

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
        }
    }
}
