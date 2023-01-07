using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class PlayerAnimatorManager: AnimatorManager
    {
        PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;

        int horizontal;
        int vertical;



        private void Awake()
        {
            anim = GetComponent<Animator>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        }

        private void Start()
        {
            horizontal = Animator.StringToHash("Horizontal");
            vertical= Animator.StringToHash("Vertical");
        }

        public void UpdateAnimatorMovementValues(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            if (isSprinting)
            {
                h = horizontalMovement;
                v = 2;
            }

            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }

        public void DisableInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        // OnAnimatorMove is a MonoBehaviour callback for processing animation movements for modifying root motion.
        // DO NOT DELETE!!!
        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting)
            {
                playerLocomotion.rb.drag = 0;

                Vector3 deltaPosition = anim.deltaPosition;
                if (playerManager.isJumping == false)
                {
                    deltaPosition.y = 0;
                }
                float delta = Time.deltaTime;
                Vector3 velocity = deltaPosition / delta;
                playerLocomotion.rb.velocity = velocity;
            }
        }
    }
}
