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
    /// <summary>
    /// Represents a Desciption Modal
    /// </summary>
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
        private List<GameObject> currentItemDescObjects = new List<GameObject>();
        private int selectedIndex;

        private Dictionary<HouseItem, List<GameObject>> items = new Dictionary<HouseItem, List<GameObject>>();

        public void Initialize()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            UIEventClose.AddListener(CloseListener);
        }
        private void OnDisable()
        {
            UIEventClose.RemoveListener(CloseListener);
        }

        private void CloseListener()
        {
            for (int i = 0; i < currentItemDescObjects.Count; i++)
            {
                currentItemDescObjects[i].SetActive(false);
            }
            gameObject.SetActive(false);
        }

        public void SetItem(HouseItem item)
        {
            currentItem = item;
            gameObject.SetActive(true);
            if (items.ContainsKey(item))
            {
                currentItemDescObjects = items[item];
            } else
            {
                currentItemDescObjects = new List<GameObject>();
                items.Add(item, currentItemDescObjects);
                for (int i = 0; i < currentItem.Objects.Count; i++)
                {
                    GameObject go;
                    if (currentItem.Objects[i].Object == null)
                    {
                        go = Instantiate(currentItem.Generic, DescParent.transform);
                        UIGenericDesc ui = go.GetComponent<UIGenericDesc>();
                        if (ui != null)
                        {
                            ui.SetData(currentItem.Objects[i]);
                        }
                    } else
                    {
                        go = Instantiate(currentItem.Objects[i].Object, DescParent.transform);
                    }
                    currentItemDescObjects.Add(go);
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

        /// <summary>
        /// Deactivates last window and activates new window based on index
        /// </summary>
        public void UpdateDisplay()
        {
            if (prevDisplayed != null)
            {
                prevDisplayed.SetActive(false);
            }
            prevDisplayed = currentItemDescObjects[selectedIndex];
            currentItemDescObjects[selectedIndex].SetActive(true);
            if (selectedIndex > 0)
            {
                DescArrowLeft.SetActive(true);
            } else
            {
                DescArrowLeft.SetActive(false);
            }
            if (selectedIndex < currentItemDescObjects.Count - 1)
            {
                DescArrowRight.SetActive(true);
            } else
            {
                DescArrowRight.SetActive(false);
            }
        }
    }
}