using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    public class UIHouseItem : MonoBehaviour
    {
        [SerializeField] HouseItem Item;
        [SerializeField] GlobalEvent PurchaseEvent;
        SpriteRenderer spriteRenderer;
        bool purchased = false;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Item.PrePurchase;
            PurchaseEvent.AddListener(purchaseEventListener);
            purchaseEventListener();
        }

        private void Update()
        {
            if (purchased)
            {
                if (Item.ResourceDependance != null)
                {
                    int index = Mathf.FloorToInt(Mathf.Clamp01(Item.ResourceDependance.Get()) / 0.95f);
                    spriteRenderer.sprite = Item.PostPurchase[index];
                    Debug.Log($"{Item.ResourceDependance.ID} {Item.ResourceDependance.Get()}");
                }
            }
        }

        private void OnDestroy()
        {
            PurchaseEvent.RemoveListener(purchaseEventListener);
        }

        private void purchaseEventListener()
        {
            if (DataSystem.GameData.Player.UpgradedItems.Contains(Item.ID))
            {
                purchased = true;
                if (Item.ResourceDependance == null)
                {
                    spriteRenderer.sprite = Item.PostPurchase[0];
                }
            }
        }

        public void OpenDesc()
        {
            if (purchased)
            {
                UIDescModal.Instance.SetItem(Item);
            }
        }

        private void OnMouseDown()
        {
            if (!Singleton.isUIOverride)
            {
                if (Item != null)
                {
                    OpenDesc();
                }
            }
        }
    }
}