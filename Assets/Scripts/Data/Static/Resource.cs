using BunnyHouse.Core;
using UnityEngine;

namespace BunnyHouse.Data
{
    /// <summary>
    /// Represents an player resource, which can be limited by maximum value
    /// </summary>
    [CreateAssetMenu(fileName = "Resource", menuName = "SO/Resource")]
    public class Resource : ScriptableObject
    {
        /// <summary>
        /// Unique identifier for the resource.
        /// </summary>
        public string ID;
        /// <summary>
        /// Enables/Disables maximum value for the resource.
        /// </summary>
        public bool HasMaxValue;
        /// <summary>
        /// Maximum value for the resource.
        /// </summary>
        public float MaxValue;
        public float Get()
        {
            return DataSystem.GameData.Player.GetResource(ID);
        }
        /// <summary>
        /// Adds value to resource
        /// </summary>
        /// <remarks>Internally clamps with maximum value if <see cref="HasMaxValue"/> is true</remarks>
        public void Add(float value)
        {
            if (HasMaxValue)
            {
                float newVal = Mathf.Clamp(Get() + value, 0f, MaxValue);
                DataSystem.GameData.Player.SetResource(ID, newVal);
            } else
            {
                DataSystem.GameData.Player.AddResource(ID, value);
            }
        }
        /// <summary>
        /// Sets value to resource
        /// </summary>
        /// <remarks>Internally clamps with maximum value if <see cref="HasMaxValue"/> is true</remarks>
        public void Set(float value)
        {
            if (HasMaxValue)
            {
                float newVal = Mathf.Clamp(value, 0f, MaxValue);
                DataSystem.GameData.Player.SetResource(ID, newVal);
            }
            else
            {
                DataSystem.GameData.Player.SetResource(ID, value);
            }
        }
    }
}