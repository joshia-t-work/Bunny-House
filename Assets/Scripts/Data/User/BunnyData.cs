using System;
using UnityEngine;

namespace BunnyHouse.Data
{
    [Serializable]
    public class BunnyData : IData
    {
        public static string FileName => "Bunny.dat";
        public string DataFileName => FileName;
        public string Name { get { return name; } }
        [SerializeField]
        private string name;
        public bool IsSick { get { return sick; } }
        [SerializeField]
        private bool sick;
        public BunnyData()
        {
            name = "";
        }

        public void SetName(string value)
        {
            name = value;
        }

        public void SetSick(bool val)
        {
            sick = val;
        }

        public void OnPause()
        {
        }
    }
}
