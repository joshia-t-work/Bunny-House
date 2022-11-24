using System;
using UnityEngine;

namespace BunnyHouse.Data
{
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
    }
}
