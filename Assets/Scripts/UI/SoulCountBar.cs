using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RY
{
    public class SoulCountBar : MonoBehaviour
    {
        public TextMeshProUGUI soulCountText;



        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }
    }
}

