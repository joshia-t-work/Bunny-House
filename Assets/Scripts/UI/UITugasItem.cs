using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    public class UITugasItem : MonoBehaviour
    {
        [SerializeField]
        Image Image;
        [SerializeField]
        TMP_Text Text;
        [SerializeField]
        GlobalEvent PurchaseEvent;
        HouseItem Item;

        public void SetData(HouseItem item)
        {
            Item = item;
            Image.sprite = item.Image;
            Text.text = item.DisplayName;
        }

        public void Buy()
        {
            if (DataSystem.GameData.Player.GetResource("Heart") >= 1)
            {
                DataSystem.GameData.Player.AddResource("Heart",-1);
                DataSystem.GameData.Player.UpgradeItem(Item.ID);
                DataSystem.SaveGame();
                PurchaseEvent.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}