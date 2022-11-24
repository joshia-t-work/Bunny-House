using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    [CreateAssetMenu(fileName = "House Item", menuName = "SO/House Item")]
    public class HouseItem : ScriptableObject
    {
        public string DisplayName;
        public Sprite Image;
        public Sprite PrePurchase;
        public Sprite[] PostPurchase;
        public Resource ResourceDependance;
        public float ResourceAddValue;
        [TextArea]
        public string Desc;
        public List<GameObject> DescObjects;
        [ReadOnly]
        public string ID;

        public HouseItem()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}