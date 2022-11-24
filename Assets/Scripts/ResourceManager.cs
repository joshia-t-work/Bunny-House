using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using BunnyHouse.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BunnyHouse.Core
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] GlobalEvent GantiLitterBox;
        [SerializeField] GlobalEvent GantiMakanan;
        [SerializeField] GlobalEvent GantiMinuman;
        const int ENERGY_REFILL_TIME = 2 * 60 * 1000;
        const float ENERGY_REFILL_RATE = 1f / ENERGY_REFILL_TIME;
        const float FOOD_DECAY_RATE = -1f / (5 * 60 * 60 * 1000);
        const float WATER_DECAY_RATE = -1f / (5 * 60 * 60 * 1000);
        const float LITTER_INCREASE_RATE = 1f / (3 * 24 * 60 * 60 * 1000);
        const int TICK_TIME = 10 * 1000;
        private void Awake()
        {
            GantiLitterBox.AddListener(gantiLitterBox);
            GantiMakanan.AddListener(gantiMakanan);
            GantiMinuman.AddListener(gantiMinuman);
        }

        private void OnDestroy()
        {
            GantiLitterBox.RemoveListener(gantiLitterBox);
            GantiMakanan.RemoveListener(gantiMakanan);
            GantiMinuman.RemoveListener(gantiMinuman);
        }

        // Start is called before the first frame update
        void Start()
        {
            int time = HouseMainThread.ticksPassed / 10000;
            Debug.Log($"{time / 1000f} passed");
            DataSystem.GameData.Player.AddResource("Energy", Mathf.FloorToInt(ENERGY_REFILL_RATE * time));
            DataSystem.GameData.Player.AddResource("Food", FOOD_DECAY_RATE * time);
            DataSystem.GameData.Player.AddResource("Water", WATER_DECAY_RATE * time);
            DataSystem.GameData.Player.AddResource("Litter", LITTER_INCREASE_RATE * time);
            DataSystem.SaveGame();
            EnergyRefill();
            TickSupplements();
        }

        private void EnergyRefill()
        {
            TaskSystem.Cancellable(async () =>
            {
                await Task.Delay(ENERGY_REFILL_TIME, TaskSystem.CancellationToken);
                DataSystem.GameData.Player.AddResource("Energy", Mathf.FloorToInt(ENERGY_REFILL_TIME * ENERGY_REFILL_RATE));
                DataSystem.SaveGame();
                EnergyRefill();
            });
        }

        private void TickSupplements()
        {
            TaskSystem.Cancellable(async () =>
            {
                await Task.Delay(TICK_TIME, TaskSystem.CancellationToken);
                DataSystem.GameData.Player.AddResource("Food", FOOD_DECAY_RATE * TICK_TIME);
                DataSystem.GameData.Player.AddResource("Water", WATER_DECAY_RATE * TICK_TIME);
                DataSystem.GameData.Player.AddResource("Litter", LITTER_INCREASE_RATE * TICK_TIME);
                DataSystem.SaveGame();
                TickSupplements();
            });
        }

        private void gantiLitterBox()
        {
            // TODO: Cost
            float cost = 0;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                DataSystem.GameData.Player.AddResource("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Litter", 0);
                DataSystem.SaveGame();
            }
        }

        private void gantiMakanan()
        {
            // TODO: Cost
            float cost = 0;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                DataSystem.GameData.Player.AddResource("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Food", 1);
                DataSystem.SaveGame();
            }
        }

        private void gantiMinuman()
        {
            // TODO: Cost
            float cost = 0;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                DataSystem.GameData.Player.AddResource("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Water", 1);
                DataSystem.SaveGame();
            }
        }
    }
}
