using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        Animator anim;

        int horizontal;
        int vertical;



        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical= Animator.StringToHash("Vertical");
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
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

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.1f);
            Debug.Log("Animation: " + targetAnim + ", isInteracting: " + isInteracting + ", playerManager: " + playerManager.isInteracting + ", anim: " + anim.GetBool("isInteracting"));
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }
    }
}
