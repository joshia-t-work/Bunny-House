using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    /// <summary>
    /// Represents a physical item to search in Minigame
    /// </summary>
    [CreateAssetMenu(fileName = "Search Item", menuName = "SO/Search Item")]
    public class SearchItem : ScriptableObject
    {
        public string ItemName;
        public Sprite Image;

        public SearchItem()
        {
        }
    }
}