using BunnyHouse.Core;
using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.UI
{
    public class UITugas : MonoBehaviour
    {
        [SerializeField]
        UITugasItem TugasPrefab;
        [SerializeField]
        List<HouseItem> HouseItems;
        [SerializeField]
        Transform Parent;
        private void Awake()
        {
            for (int i = 0; i < HouseItems.Count; i++)
            {
                if (!DataSystem.GameData.Player.UpgradedItems.Contains(HouseItems[i].ID))
                {
                    Instantiate(TugasPrefab, Parent).GetComponent<UITugasItem>().SetData(HouseItems[i]);
                }
            }
        }
    }
}