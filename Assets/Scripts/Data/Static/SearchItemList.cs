using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    /// <summary>
    /// Represents a list of search item
    /// </summary>
    [CreateAssetMenu(fileName = "Search Item List", menuName = "SO/Search Item List")]
    public class SearchItemList : ScriptableObject
    {
        public SearchItem[] Items;

        public SearchItemList()
        {
        }
    }
}