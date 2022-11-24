using BunnyHouse.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data
{
    [CreateAssetMenu(fileName = "Resource", menuName = "SO/Resource")]
    public class Resource : ScriptableObject
    {
        public string ID;
        public bool HasMaxValue;
        public float MaxValue;
        public float Get()
        {
            return DataSystem.GameData.Player.GetResource(ID);
        }
        public void Add(float value)
        {
            DataSystem.GameData.Player.AddResource(ID, value);
        }
        public void Set(float value)
        {
            DataSystem.GameData.Player.SetResource(ID, value);
        }
    }
}