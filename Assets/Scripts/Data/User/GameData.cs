using System;
using UnityEngine;

namespace BunnyHouse.Data
{
    /// <summary>
    /// Represents game-related data
    /// </summary>
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

        /// <summary>
        /// Represents time since last save was opened for time-spent outside game in ticks
        /// </summary>
        public long LastTicksOpen { get { return lastTicksOpen; } }
        [SerializeField]
        private long lastTicksOpen = 0;
        
        public GameData()
        {
            lastTicksOpen = DateTime.Now.Ticks;

            player = new PlayerData();
            bunny = new BunnyData();
            tutorial = new TutorialData();
        }

        /// <summary>
        /// Performs operations during saving or mouse off-screen or app paused
        /// </summary>
        public void OnPause()
        {
            lastTicksOpen = DateTime.Now.Ticks;
            player.OnPause();
            bunny.OnPause();
            tutorial.OnPause();
        }
    }
}
