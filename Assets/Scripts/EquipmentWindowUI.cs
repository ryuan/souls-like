using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot1Selected;
        public bool rightHandSlot2Selected;
        public bool rightHandSlot3Selected;
        public bool rightHandSlot4Selected;
        public bool leftHandSlot1Selected;
        public bool leftHandSlot2Selected;
        public bool leftHandSlot3Selected;
        public bool leftHandSlot4Selected;

        HandEquipmentSlotUI[] handEquipmentSlotUI;

        private void Start()
        {
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].rightHandSlot1)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[0]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot2)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[1]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot3)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[2]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot4)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[3]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot1)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[0]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot2)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[1]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot3)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[2]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot4)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[3]);
                }
            }
        }

        public void SelectRightHandSlot1()
        {
            rightHandSlot1Selected = true;
        }

        public void SelectRightHandSlot2()
        {
            rightHandSlot2Selected = true;
        }

        public void SelectRightHandSlot3()
        {
            rightHandSlot3Selected = true;
        }

        public void SelectRightHandSlot4()
        {
            rightHandSlot4Selected = true;
        }

        public void SelectLeftHandSlot1()
        {
            leftHandSlot1Selected = true;
        }

        public void SelectLeftHandSlot2()
        {
            leftHandSlot2Selected = true;
        }

        public void SelectLeftHandSlot3()
        {
            leftHandSlot3Selected = true;
        }

        public void SelectLeftHandSlot4()
        {
            leftHandSlot4Selected = true;
        }
    }

}
