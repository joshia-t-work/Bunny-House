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
    public class UIDescModal : MonoBehaviour
    {
        public static UIDescModal Instance { get; private set; }

        [SerializeField]
        private GameObject DescParent;

        [SerializeField]
        private GameObject DescArrowLeft;

        [SerializeField]
        private GameObject DescArrowRight;

        [SerializeField]
        private GlobalEvent UIEventClose;

        private GameObject prevDisplayed = null;
        private HouseItem currentItem;
        private List<GameObject> currentItemObjects = new List<GameObject>();
        private int selectedIndex;

        private Dictionary<HouseItem, List<GameObject>> items = new Dictionary<HouseItem, List<GameObject>>();

        public void Initialize()
        {
            Instance = this;
            UIEventClose.AddListener(CloseListener);
        }
        private void OnDestroy()
        {
            UIEventClose.RemoveListener(CloseListener);
        }

        private void CloseListener()
        {
            for (int i = 0; i < currentItemObjects.Count; i++)
            {
                currentItemObjects[i].SetActive(false);
            }
            gameObject.SetActive(false);
        }

        public void SetItem(HouseItem item)
        {
            currentItem = item;
            gameObject.SetActive(true);
            if (items.ContainsKey(item))
            {
                currentItemObjects = items[item];
            } else
            {
                currentItemObjects = new List<GameObject>();
                items.Add(item, currentItemObjects);
                for (int i = 0; i < currentItem.DescObjects.Count; i++)
                {
                    GameObject go = Instantiate(currentItem.DescObjects[i], DescParent.transform);
                    UIGenericDesc ui = go.GetComponent<UIGenericDesc>();
                    if (ui != null)
                    {
                        ui.SetData(currentItem);
                    }
                    currentItemObjects.Add(go);
                }
            }
            selectedIndex = 0;
            UpdateDisplay();
        }

        public void SelectRight()
        {
            selectedIndex += 1;
            UpdateDisplay();
        }

        public void SelectLeft()
        {
            selectedIndex -= 1;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (prevDisplayed != null)
            {
                prevDisplayed.SetActive(false);
            }
            prevDisplayed = currentItemObjects[selectedIndex];
            currentItemObjects[selectedIndex].SetActive(true);
            if (selectedIndex > 0)
            {
                DescArrowLeft.SetActive(true);
            } else
            {
                DescArrowLeft.SetActive(false);
            }
            if (selectedIndex < currentItemObjects.Count - 1)
            {
                DescArrowRight.SetActive(true);
            } else
            {
                DescArrowRight.SetActive(false);
            }
        }
    }
}