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
    /// <summary>
    /// Represents a Tugas UI item
    /// </summary>
    public class UITugasItem : MonoBehaviour
    {
        [SerializeField]
        Image Image;
        [SerializeField]
        TMP_Text Text;
        [SerializeField]
        GlobalEventSO PurchaseEvent;
        HouseItem Item;

        [SerializeField]
        GameObject[] Tutorials;

        public void SetData(HouseItem item)
        {
            Item = item;
            Image.sprite = item.Image;
            Text.text = item.DisplayName;
        }

        /// <summary>
        /// Darkens everything except buy button
        /// </summary>
        public void SetTutorial()
        {
            for (int i = 0; i < Tutorials.Length; i++)
            {
                Tutorials[i].SetActive(true);
            }
        }

        /// <remarks>For UI calls</remarks>
        public void Buy()
        {
            if (DataSystem.GameData.Player.GetResource("Heart") >= 1)
            {
                DataSystem.GameData.Player.AddResource("Heart",-1);
                DataSystem.GameData.Player.UpgradeItem(Item.ID);
                DataSystem.SaveGame();
                PurchaseEvent.Invoke(Item);
                gameObject.SetActive(false);
            }
        }
    }
}