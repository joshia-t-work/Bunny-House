using BunnyHouse.Data;
using BunnyHouse.Data.Events;
using System;
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
        [SerializeField] GameObject TimesUp;
        [SerializeField] Resource[] Resources;
        const int ENERGY_REFILL_TIME = 2 * 60 * 1000;
        const float ENERGY_REFILL_RATE = 1f / ENERGY_REFILL_TIME;
        const float FOOD_DECAY_RATE = -1f / (5 * 60 * 60 * 1000);
        const float WATER_DECAY_RATE = -1f / (5 * 60 * 60 * 1000);
        const float LITTER_INCREASE_RATE = 1f / (3 * 24 * 60 * 60 * 1000);
        const float TIME_DECAY_RATE = -1f / 1000;
        const int TICK_TIME = 1000;
        Dictionary<string, Resource> _resources = new Dictionary<string, Resource>();
        private void Awake()
        {
            for (int i = 0; i < Resources.Length; i++)
            {
                _resources.Add(Resources[i].ID, Resources[i]);
            }
            try
            {
                _resources["Time"].Set(0f);
            }
            catch (Exception)
            {

                throw;
            }
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
            long time = HouseMainThread.ticksPassed / 10000;
            TimeSkip(time);
            EnergyRefill();
            TickSupplements();
            TickTime();
        }

        public void TimeSkip(long millisecondsPassed)
        {
            PlayerDataAdd("Energy", Mathf.FloorToInt(ENERGY_REFILL_RATE * millisecondsPassed));
            DataSystem.GameData.Player.AddResource("Food", FOOD_DECAY_RATE * millisecondsPassed);
            DataSystem.GameData.Player.AddResource("Water", WATER_DECAY_RATE * millisecondsPassed);
            DataSystem.GameData.Player.AddResource("Litter", LITTER_INCREASE_RATE * millisecondsPassed);
            DataSystem.SaveGame();
        }

        private void EnergyRefill()
        {
            TaskSystem.Cancellable(async () =>
            {
                await Task.Delay(ENERGY_REFILL_TIME, TaskSystem.CancellationToken);
                PlayerDataAdd("Energy", Mathf.FloorToInt(ENERGY_REFILL_TIME * ENERGY_REFILL_RATE));
                DataSystem.SaveGame();
                EnergyRefill();
            });
        }

        private void TickSupplements()
        {
            TaskSystem.Cancellable(async () =>
            {
                await Task.Delay(TICK_TIME * 10, TaskSystem.CancellationToken);
                DataSystem.GameData.Player.AddResource("Food", FOOD_DECAY_RATE * TICK_TIME * 10);
                DataSystem.GameData.Player.AddResource("Water", WATER_DECAY_RATE * TICK_TIME * 10);
                DataSystem.GameData.Player.AddResource("Litter", LITTER_INCREASE_RATE * TICK_TIME * 10);
                DataSystem.SaveGame();
                TickSupplements();
            });
        }

        private void TickTime()
        {
            TaskSystem.Cancellable(async () =>
            {
                await Task.Delay(TICK_TIME, TaskSystem.CancellationToken);
                if (_resources["Time"].Get() > 0)
                {
                    _resources["Time"].Add(TIME_DECAY_RATE * TICK_TIME);
                    if (_resources["Time"].Get() <= 0)
                    {
                        TimesUp.SetActive(true);
                    }
                }
                TickTime();
            });
        }

        private void gantiLitterBox()
        {
            float cost = 20;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                PlayerDataAdd("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Litter", 0);
                DataSystem.SaveGame();
            }
        }

        private void gantiMakanan()
        {
            float cost = 20;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                PlayerDataAdd("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Food", 1);
                DataSystem.SaveGame();
            }
        }

        private void gantiMinuman()
        {
            float cost = 20;
            if (DataSystem.GameData.Player.GetResource("Coin") >= cost)
            {
                PlayerDataAdd("Coin", -cost);
                DataSystem.GameData.Player.SetResource("Water", 1);
                DataSystem.SaveGame();
            }
        }
        
        public float PlayerDataGet(string resource)
        {
            try
            {
                return _resources[resource].Get();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            return 0f;
        }

        public void PlayerDataAdd(string resource, float value)
        {
            try
            {
                _resources[resource].Add(value);
                DataSystem.GameData.Player.SetResource(resource, _resources[resource].Get());
            }
            catch (Exception e) {
                Debug.LogWarning(e);
            }
        }

        public void PlayerDataSet(string resource, float value)
        {
            try
            {
                _resources[resource].Set(value);
                DataSystem.GameData.Player.SetResource(resource, _resources[resource].Get());
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}
