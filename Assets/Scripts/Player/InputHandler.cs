using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls inputActions;

        PlayerAttackHandler playerAttackHandler;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        WeaponHolderSlotManager weaponHolderSlotManager;
        PlayerAnimatorManager animatorManager;
        UIManager uiManager;

        [Header("Player & Camera Movement Inputs")]
        Vector2 movementInput;
        Vector2 cameraInput;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        [Header("Player Action Inputs")]
        public bool b_Input;
        public bool a_Input;
        public bool y_Input;
        public bool lt_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool critAtk_Input;
        public bool jump_Input;

        public float rollInputTimer;

        [Header("Inventory & Quick Slot Inputs")]
        public bool dPad_Up_Input;
        public bool dPad_Down_Input;
        public bool dPad_Left_Input;
        public bool dPad_Right_Input;
        public bool inventory_Input;

        [Header("Target Lock-On Inputs")]
        public bool lockOn_Input;
        public bool rStick_Left_Input;
        public bool rStick_Right_Input;

        [Header("Action Determinant Flags")]
        public bool rollFlag;
        public bool sprintFlag;
        public bool twoHandFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;



        private void Awake()
        {
            playerAttackHandler = GetComponentInChildren<PlayerAttackHandler>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponHolderSlotManager = GetComponentInChildren<WeaponHolderSlotManager>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += context => cameraInput = context.ReadValue<Vector2>();
                inputActions.PlayerActions.A.performed += context => a_Input = true;
                inputActions.PlayerActions.Y.performed += context => y_Input = true;
                inputActions.PlayerActions.LT.performed += context => lt_Input = true;
                inputActions.PlayerActions.RB.performed += context => rb_Input = true;
                inputActions.PlayerActions.RT.performed += context => rt_Input = true;
                inputActions.PlayerActions.CriticalAttack.performed += context => critAtk_Input = true;
                inputActions.PlayerActions.Jump.performed += context => jump_Input = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += context => dPad_Left_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += context => dPad_Right_Input = true;
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
            MoveInput();
            HandleRollAndSprintInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticleAttackInput();
        }

        private void MoveInput()
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
            // LT input handles the LEFT hand weapon's light attack
            // or weapon art if (1) LEFT hand weapon is a shield or (2) TWO handed weapon
            if (lt_Input)
            {
                if (twoHandFlag)
                {

                }
                else
                {
                    playerAttackHandler.HandleLTAction();
                }
            }

            // RB input handles the RIGHT hand weapon's light attack
            if (rb_Input)
            {
                playerAttackHandler.HandleRBAction();
            }

            // RT input handles the RIGHT hand weapon's heavy attack
            if (rt_Input)
            {
                if (playerManager.canDoCombo)
                {
                    animatorManager.anim.SetBool("usingRightWeapon", true);
                    playerAttackHandler.HandleWeaponCombo(playerInventory.rightWeapon);
                }
                else
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorManager.anim.SetBool("usingRightWeapon", true);
                        playerAttackHandler.HandleHeavyAttack(playerInventory.rightWeapon);
                    }
                }
            }
        }

        private void HandleQuickSlotInput()
        {
            if (dPad_Left_Input)
            {
                playerInventory.ChangeWeapon(true);
            }
            else if (dPad_Right_Input)
            {
                playerInventory.ChangeWeapon(false);
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.SetActiveSelectWindow(true);
                    uiManager.UpdateWeaponInventoryUI();
                    uiManager.SetActiveHUDWindow(false);
                }
                else
                {
                    uiManager.SetActiveSelectWindow(false);
                    uiManager.CloseAllOpenMenuWindows();
                    uiManager.SetActiveHUDWindow(true);
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

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponHolderSlotManager.LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponHolderSlotManager.LoadWeaponOnHolderSlot(playerInventory.leftWeapon, true);
                    weaponHolderSlotManager.LoadWeaponOnHolderSlot(playerInventory.rightWeapon, false);
                }
            }
        }

        private void HandleCriticleAttackInput()
        {
            if (critAtk_Input)
            {
                critAtk_Input = false;
                playerAttackHandler.HandleCriticalAttacks();
            }
        }
    }
}
