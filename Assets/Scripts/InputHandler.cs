using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public float rollInputTimer;

        public bool b_Input;
        public bool a_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;

        public bool dPad_Up_Input;
        public bool dPad_Down_Input;
        public bool dPad_Left_Input;
        public bool dPad_Right_Input;
        public bool inventory_Input;

        public bool lockOn_Input;
        public bool rStick_Left_Input;
        public bool rStick_Right_Input;

        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        UIManager uiManager;

        Vector2 movementInput;
        Vector2 cameraInput;



        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += context => cameraInput = context.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += context => rb_Input = true;
                inputActions.PlayerActions.RT.performed += context => rt_Input = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += context => dPad_Left_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += context => dPad_Right_Input = true;
                inputActions.PlayerActions.A.performed += context => a_Input = true;
                inputActions.PlayerActions.Jump.performed += context => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += context => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += context => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += context => rStick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += context => rStick_Right_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollAndSprintInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollAndSprintInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.IsPressed();

            if (b_Input)
            {
                rollInputTimer += delta;

                if (moveAmount > 0 && rollInputTimer > 0.5f)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                    sprintFlag = false;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            // RB input handles the RIGHT hand weapon's light attack
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                    {
                        return;
                    }
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            // RT input handles the RIGHT hand weapon's heavy attack
            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotInput()
        {
            if (dPad_Left_Input)
            {
                playerInventory.ChangeLeftWeapon();
            }
            else if (dPad_Right_Input)
            {
                playerInventory.ChangeRightWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                cameraHandler.HandleLockOn();

                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag == true) {
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag)
            {
                if (rStick_Left_Input)
                {
                    cameraHandler.HandleLockOn();

                    if (cameraHandler.leftLockTarget != null)
                    {
                        cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                    }
                }
                else if (rStick_Right_Input)
                {
                    cameraHandler.HandleLockOn();

                    if (cameraHandler.rightLockTarget != null)
                    {
                        cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                    }
                }
            }

            cameraHandler.SetCameraHeight();
        }
    }
}
