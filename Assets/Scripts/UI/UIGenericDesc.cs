using BunnyHouse.Core;
using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    public class UIGenericDesc : MonoBehaviour
    {
        [SerializeField] Image Image;
        [SerializeField] TMP_Text Text;
        public void SetData(HouseItem item)
        {
            Image.sprite = item.Image;
            Text.text = item.Desc;
        }
    }
}