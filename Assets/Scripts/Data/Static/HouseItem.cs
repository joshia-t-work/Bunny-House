using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    /// <summary>
    /// Represents an Item on the house
    /// </summary>
    [CreateAssetMenu(fileName = "House Item", menuName = "SO/House Item")]
    public class HouseItem : ScriptableObject
    {
        /// <summary>
        /// Name to be displayed on purchase
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// Icon of the item
        /// </summary>
        public Sprite Image;
        /// <summary>
        /// Physical item sprite before purchased
        /// </summary>
        public Sprite PrePurchase;
        /// <summary>
        /// Array of post-purchase sprites, multiple indicating the item's sprite depends on the resource value
        /// </summary>
        public Sprite[] PostPurchase;
        /// <summary>
        /// Resource this item depends on, null if not resource-dependant
        /// </summary>
        public Resource ResourceDependance;
        /// <summary>
        /// Generic description prefab when this item is clicked, see <see cref="DescriptiveObject"/>
        /// </summary>
        public GameObject Generic;
        /// <summary>
        /// Custom description windows when this item is clicked
        /// </summary>
        public List<DescriptiveObject> Objects;
        /// <summary>
        /// Unique ID for this item, only generated on creation
        /// </summary>
        [ReadOnly]
        public string ID;

        public HouseItem()
        {
            ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Represents a description prefab with an image and text. Uses Generic GameObject by default.
        /// </summary>
        [Serializable]
        public class DescriptiveObject
        {
            /// <summary>
            /// When null, uses Generic GameObject to create an image with description, else ignores Desc and Image
            /// </summary>
            [Header("Pick one: Object / ImageDesc")]
            public GameObject Object;
            [TextArea]
            public string Desc;
            public Sprite Image;
        }
    }
}