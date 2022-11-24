using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data
{
    [Serializable]
    public class PlayerData : IData
    {
        public static string FileName => "Player.dat";
        public string DataFileName => FileName;
        public string Name { get { return name; } }
        [SerializeField]
        private string name;
        public List<string> UpgradedItems { get; private set; } = new List<string>();
        [SerializeField]
        private List<string> resourceNames = new List<string>();
        [SerializeField]
        private List<float> resourceValues = new List<float>();

        public PlayerData()
        {
            name = "";
        }

        public void SetName(string value)
        {
            name = value;
        }
        public float GetResource(string resourceName)
        {
            if (resourceNames.Contains(resourceName))
            {
                return resourceValues[resourceNames.IndexOf(resourceName)];
            } else
            {
                resourceNames.Add(resourceName);
                resourceValues.Add(0);
                return 0;
            }
        }
        public void AddResource(string resourceName, float value)
        {
            GetResource(resourceName);
            resourceValues[resourceNames.IndexOf(resourceName)] += value;
        }
        public void SetResource(string resourceName, float value)
        {
            GetResource(resourceName);
            resourceValues[resourceNames.IndexOf(resourceName)] = value;
        }
        public void UpgradeItem(string itemID)
        {
            if (!UpgradedItems.Contains(itemID))
            {
                UpgradedItems.Add(itemID);
            }
        }

        public void OnPause()
        {
        }
    }
}
