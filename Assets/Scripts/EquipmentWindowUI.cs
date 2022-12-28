using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RY
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public Slot selectedSlot;

        HandEquipmentSlotUI[] handEquipmentSlotUI;



        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();

            Debug.Log("Loading weapons to UI Equipment Screen Window");
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                switch (handEquipmentSlotUI[i].handSlot)
                {
                    case Slot.LEFT1:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[0]);
                        break;
                    case Slot.LEFT2:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[1]);
                        break;
                    case Slot.LEFT3:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[2]);
                        break;
                    case Slot.LEFT4:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[3]);
                        break;
                    case Slot.RIGHT1:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[0]);
                        break;
                    case Slot.RIGHT2:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[1]);
                        break;
                    case Slot.RIGHT3:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[2]);
                        break;
                    case Slot.RIGHT4:
                        handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[3]);
                        break;
                    default:
                        break;
                }
            }
        }

        public void SelectLeftHandSlot1()
        {
            selectedSlot = Slot.LEFT1;
        }

        public void SelectLeftHandSlot2()
        {
            selectedSlot = Slot.LEFT2;
        }

        public void SelectLeftHandSlot3()
        {
            selectedSlot = Slot.LEFT3;
        }

        public void SelectLeftHandSlot4()
        {
            selectedSlot = Slot.LEFT4;
        }

        public void SelectRightHandSlot1()
        {
            selectedSlot = Slot.RIGHT1;
        }

        public void SelectRightHandSlot2()
        {
            selectedSlot = Slot.RIGHT2;
        }

        public void SelectRightHandSlot3()
        {
            selectedSlot = Slot.RIGHT3;
        }

        public void SelectRightHandSlot4()
        {
            selectedSlot = Slot.RIGHT4;
        }
    }

}
