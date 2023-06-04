using System;
using UnityEngine;

namespace BunnyHouse.Data
{
    /// <summary>
    /// Represents tutorial-related data
    /// </summary>
    [Serializable]
    public class TutorialData : IData
    {
        public static string FileName => "Tutorial.dat";
        public string DataFileName => FileName;
        public bool CariBarang { get { return cariBarang; } }
        [SerializeField]
        private bool cariBarang;
        public bool BeliBarang { get { return beliBarang; } }
        [SerializeField]
        private bool beliBarang;
        public bool Dock { get { return dock; } }
        [SerializeField]
        private bool dock;
        public bool Hint { get { return hint; } }
        [SerializeField]
        private bool hint;

        public TutorialData()
        {
        }

        public void OnPause()
        {
        }

        public void FinishCariBarang()
        {
            cariBarang = true;
        }
        public void FinishBeliBarang()
        {
            beliBarang = true;
        }
        public void FinishDock()
        {
            dock = true;
        }
        public void FinishHint()
        {
            hint = true;
        }
    }
}
