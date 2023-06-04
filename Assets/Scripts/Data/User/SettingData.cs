using System;
using UnityEngine;

namespace BunnyHouse.Data
{
    /// <summary>
    /// Represents setting-related data
    /// </summary>
    [Serializable]
    public class SettingData : IData
    {
        public static string FileName => "Settings.dat";
        public string DataFileName => FileName;
        public float Music { get { return music; } }
        [SerializeField]
        private float music;
        public float Sound { get { return sound; } }
        [SerializeField]
        private float sound;

        public void SetMusic(float value)
        {
            music = value;
        }
        public void SetSound(float value)
        {
            sound = value;
        }
    }
}
