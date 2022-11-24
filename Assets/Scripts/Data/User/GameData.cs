using System;
using UnityEngine;

namespace BunnyHouse.Data
{
    [Serializable]
    public class GameData : IData
    {
        public static string FileName => "Game.dat";
        public string DataFileName => FileName;
        public PlayerData Player { get { return player; } }
        [SerializeField]
        private PlayerData player;
        public BunnyData Bunny { get { return bunny; } }
        [SerializeField]
        private BunnyData bunny;

        public TutorialData Tutorial { get { return tutorial; } }
        [SerializeField]
        private TutorialData tutorial;

        public int LastTicksOpen { get { return lastTicksOpen; } }
        [SerializeField]
        private int lastTicksOpen = 0;

        public GameData()
        {
            DateTime dateTime = DateTime.Now;
            lastTicksOpen = (int)dateTime.Ticks;

            player = new PlayerData();
            bunny = new BunnyData();
            tutorial = new TutorialData();
        }

        public void OnPause()
        {
            DateTime dateTime = DateTime.Now;
            lastTicksOpen = (int)dateTime.Ticks;
            player.OnPause();
            bunny.OnPause();
            tutorial.OnPause();
        }
    }
}
