using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class Interactable : MonoBehaviour
    {
        public string interactableText;



        public virtual void Interact()
        {
            Debug.Log("You interacted with an object");
        }
    }

}
