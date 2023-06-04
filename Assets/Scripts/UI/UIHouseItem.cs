using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using UnityEngine;

namespace BunnyHouse.UI
{
    /// <summary>
    /// Represents a physical item in the house, clickable by the player
    /// </summary>
    public class UIHouseItem : MonoBehaviour
    {
        [SerializeField] HouseItem Item;
        [SerializeField] GlobalEventSO PurchaseEvent;
        ParticleSystem ps;
        SpriteRenderer spriteRenderer;
        bool purchased = false;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Item.PrePurchase;
            PurchaseEvent.AddListener(purchaseEventListener);

            ps = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            RefreshDisplay();
        }

        private void Update()
        {
            if (purchased)
            {
                if (Item.ResourceDependance != null)
                {
                    int index = Mathf.RoundToInt(Mathf.Clamp01(Item.ResourceDependance.Get()) * (Item.PostPurchase.Length -1 ));
                    spriteRenderer.sprite = Item.PostPurchase[index];
                    //Debug.Log($"{Item.ResourceDependance.ID} {Item.ResourceDependance.Get()}");
                }
            }
        }

        private void OnDestroy()
        {
            PurchaseEvent.RemoveListener(purchaseEventListener);
        }

        /// <summary>
        /// Update visuals on purchasing this item
        /// </summary>
        /// <param name="so"></param>
        private void purchaseEventListener(ScriptableObject so)
        {
            if (so == Item)
            {
                FocusThis();
            }
            RefreshDisplay();
        }

        /// <summary>
        /// Refresh sprite on purchase
        /// </summary>
        private void RefreshDisplay()
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

        /// <summary>
        /// Opens description modal
        /// </summary>
        public void OpenDesc()
        {
            if (purchased)
            {
                UIDescModal.Instance.SetItem(Item);
            }
        }

        private void OnMouseUpAsButton()
        {
            if (!Singleton.isUIOverride())
            {
                if (Item != null)
                {
                    OpenDesc();
                }
            }
        }

        private void FocusThis()
        {
            CameraScript.FocusOn(transform);
            ps.Play();
        }
    }
}