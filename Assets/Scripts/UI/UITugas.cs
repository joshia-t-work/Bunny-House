using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    /// <summary>
    /// Represents a Tugas UI
    /// </summary>
    public class UITugas : MonoBehaviour
    {
        [SerializeField]
        UITugasItem TugasPrefab;
        [SerializeField]
        HouseItem Tutorial;
        [SerializeField]
        Mask TutorialMask;
        [SerializeField]
        List<HouseItem> HouseItems;
        [SerializeField]
        List<HouseItem> HouseItemsTier2;
        [SerializeField]
        Transform Parent;
        [SerializeField]
        GlobalEventSO PurchaseEvent;
        /// <summary>
        /// Filled with Tutorial UI instances, required to unlock tier 1
        /// </summary>
        List<UITugasItem> tutItem = new List<UITugasItem>();
        /// <summary>
        /// Filled with HouseItems UI instances, required to unlock tier 2
        /// </summary>
        List<UITugasItem> tier1Item = new List<UITugasItem>();
        /// <summary>
        /// Filled with HouseItemTier2 UI instances
        /// </summary>
        List<UITugasItem> tier2Item = new List<UITugasItem>();

        private void Awake()
        {
            {
                UITugasItem item = Instantiate(TugasPrefab, Parent).GetComponent<UITugasItem>();
                item.SetData(Tutorial);
                item.SetTutorial();
                tutItem.Add(item);
            }

            for (int i = 0; i < HouseItems.Count; i++)
            {
                UITugasItem item = Instantiate(TugasPrefab, Parent).GetComponent<UITugasItem>();
                item.SetData(HouseItems[i]);
                tier1Item.Add(item);
            }

            for (int i = 0; i < HouseItemsTier2.Count; i++)
            {
                UITugasItem item = Instantiate(TugasPrefab, Parent).GetComponent<UITugasItem>();
                item.SetData(HouseItemsTier2[i]);
                tier2Item.Add(item);
            }

            PurchaseEvent.AddListener(onPurchase);
        }

        private void OnDestroy()
        {
            PurchaseEvent.RemoveListener(onPurchase);
        }

        private void onPurchase(ScriptableObject obj)
        {
            gameObject.SetActive(false);
        }

        /// <remarks>DEVLOPMENT PURPOSES</remarks>
        public void UnlockAll()
        {
            for (int i = 0; i < DataSystem.GameData.Player.UpgradedItems.Count; i++)
            {
                Debug.Log(DataSystem.GameData.Player.UpgradedItems[i]);
            }
            if (!DataSystem.GameData.Player.UpgradedItems.Contains(Tutorial.ID))
            {
                DataSystem.GameData.Player.UpgradeItem(Tutorial.ID);
                PurchaseEvent.Invoke(Tutorial);
            }

            for (int i = 0; i < HouseItems.Count; i++)
            {
                Debug.Log(HouseItems[i].ID);
                Debug.Log(DataSystem.GameData.Player.UpgradedItems.Contains(HouseItems[i].ID));
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItems[i].ID))
                {
                    DataSystem.GameData.Player.UpgradeItem(HouseItems[i].ID);
                    PurchaseEvent.Invoke(HouseItems[i]);
                }
            }
            
            for (int i = 0; i < HouseItemsTier2.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItemsTier2[i].ID))
                {
                    DataSystem.GameData.Player.UpgradeItem(HouseItemsTier2[i].ID);;
                    PurchaseEvent.Invoke(HouseItemsTier2[i]);
                }
            }
            for (int i = 0; i < DataSystem.GameData.Player.UpgradedItems.Count; i++)
            {
                Debug.Log(DataSystem.GameData.Player.UpgradedItems[i]);
            }
        }

        /// <summary>
        /// Reactivate the corresponding tugas tiers that has not been unlocked
        /// </summary>
        private void OnEnable()
        {
            TutorialMask.enabled = true;
            for (int i = 0; i < tutItem.Count; i++)
            {
                tutItem[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < tier1Item.Count; i++)
            {
                tier1Item[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < tier2Item.Count; i++)
            {
                tier2Item[i].gameObject.SetActive(false);
            }
            
            for (int i = 0; i < tutItem.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(Tutorial.ID))
                {
                    TutorialMask.enabled = false;
                    tutItem[i].gameObject.SetActive(true);
                    return;
                }
            }
            
            for (int i = 0; i < tier1Item.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItems[i].ID))
                {
                    tier1Item[i].gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < tier1Item.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItems[i].ID))
                {
                    return;
                }
            }
            
            for (int i = 0; i < tier2Item.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItemsTier2[i].ID))
                {
                    tier2Item[i].gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < tier2Item.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItemsTier2[i].ID))
                {
                    return;
                }
            }
        }
    }
}