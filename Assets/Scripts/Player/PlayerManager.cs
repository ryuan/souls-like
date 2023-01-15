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
        PlayerInteractions playerInteractions;
        PlayerStats playerStats;
        Animator anim;



        [Header("State Flags")]
        public bool isInteracting;
        public bool isGrounded;
        public bool isFalling;
        public bool isJumping;
        public bool canRotate;
        public bool canDoCombo;
        public bool usingRightWeapon;
        public bool usingLeftWeapon;
        public bool isInvulnerable;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerInteractions = GetComponent<PlayerInteractions>();
            playerStats = GetComponent<PlayerStats>();
            anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            // Don't let colliders of the same character to bump into each other
            Physics.IgnoreCollision(mainCollider, collisionBlockerCollider, true);
        }

        // Update is called every frame, before LateUpdate but after FixedUpdate.
        // Put any functions that either updates/relies on inputs here.
        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            anim.applyRootMotion = isInteracting;
            canRotate = anim.GetBool("canRotate");
            canDoCombo = anim.GetBool("canDoCombo");
            usingRightWeapon = anim.GetBool("usingRightWeapon");
            usingLeftWeapon = anim.GetBool("usingLeftWeapon");
            isInvulnerable = anim.GetBool("isInvulnerable");
            anim.SetBool("isInAir", isFalling);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRoll();
            playerLocomotion.HandleJumping();
            playerStats.RegenStamina();

            playerInteractions.CheckForInteractables();
        }

        // FixedUpdate gets called every fixed frames.
        // On frames where it's called, FixedUpdate executes before both Update and LateUpdate!
        // Put any functions that call on Unity's physics engine to affect rigidbodies here.
        private void FixedUpdate()
        {
            playerLocomotion.HandleMovementAndSprint();
            playerLocomotion.HandleRotation();
            playerLocomotion.HandleFalling();
            // Need to reset flag in FixedUpdate to match update timing of HandleMovementAndSprint
            inputHandler.sprintFlag = false;
        }

        // LateUpdate is called every frame after all other update functions have finished executing.
        // Put any camera-related functions here so it tracks all objects after they've finished moving.
        // Additionally, any flags/variables that were updated in Update should be reset here.
        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.a_Input = false;
            inputHandler.y_Input = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.jump_Input = false;

            inputHandler.dPad_Up_Input = false;
            inputHandler.dPad_Down_Input = false;
            inputHandler.dPad_Left_Input = false;
            inputHandler.dPad_Right_Input = false;
            inputHandler.inventory_Input = false;
            
            inputHandler.lockOn_Input = false;
            inputHandler.rStick_Left_Input = false;
            inputHandler.rStick_Right_Input = false;

            if (cameraHandler != null)
            {
                float fixedDelta = Time.fixedDeltaTime;
                cameraHandler.FollowTarget(fixedDelta);
                cameraHandler.HandleCameraRotation(fixedDelta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isFalling)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }
    }
}
