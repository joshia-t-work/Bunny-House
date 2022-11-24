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
    public class HouseMainThread : MonoBehaviour
    {
        [SerializeField] UIPopup UIPopup;
        [SerializeField] UIPrologue UIPrologue;
        [SerializeField] UIDialogue UIDialogue;
        [SerializeField] UIDescModal UIDescModal;
        [SerializeField] UISickModal UISickModal;
        [SerializeField] List<DialogueScene> Scenes;
        [SerializeField] GameObject GameUI;
        [SerializeField] GameObject MinigameUI;
        [SerializeField] GameObject TutorialMain;
        [SerializeField] GameObject TutorialBeli;
        [SerializeField] GlobalEvent PurchaseEvent;
        private TaskCompletionSource<bool> GameEnd = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickMainCariBarang = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickBukuTugas = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> BeliBarang = new TaskCompletionSource<bool>();
        public static int ticksPassed;

        private void Awake()
        {
            DateTime dateTime = DateTime.Now;
            int ticks = (int)dateTime.Ticks;
            ticksPassed = ticks - DataSystem.GameData.LastTicksOpen;
            if (ticksPassed / 10000000 / 60 / 60 / 24 > 3)
            {
                DataSystem.GameData.Bunny.SetSick(true);
            }
            PurchaseEvent.AddListener(purchaseEvent);
        }

        private void OnDestroy()
        {
            PurchaseEvent.RemoveListener(purchaseEvent);
        }

        // Start is called before the first frame update
        void Start()
        {
            UIDescModal.Initialize();
            _ = TaskSystem.Run(async () =>
            {
                if (DataSystem.GameData.Player.Name == "")
                {
                    DataSystem.GameData.Player.SetName(await UIPopup.ShowPrompt("Siapa Namamu?"));
                    await UIPrologue.RunPrologue(Scenes[0]);
                    DataSystem.SaveGame();
                }
                if (DataSystem.GameData.Bunny.Name == "")
                {
                    await UIDialogue.RunDialogue(Scenes[1]);
                    DataSystem.GameData.Bunny.SetName(await UIPopup.ShowPrompt("Siapa Nama Kelincimu?"));
                    DataSystem.SaveGame();
                }
                if (!DataSystem.GameData.Tutorial.CariBarang)
                {
                    DataSystem.GameData.Player.SetResource("Energy", 60);
                    await UIDialogue.RunDialogue(Scenes[2]);
                    TaskSystem.Main.Run(() => TutorialMain.SetActive(true));
                    ClickMainCariBarang = new TaskCompletionSource<bool>();
                    await ClickMainCariBarang.Task;
                    TaskSystem.Main.Run(() => TutorialMain.SetActive(false));
                    await UIDialogue.RunDialogue(Scenes[3]);
                    await GameEnd.Task;
                    DataSystem.GameData.Tutorial.FinishCariBarang();
                    DataSystem.SaveGame();
                }
                if (!DataSystem.GameData.Tutorial.BeliBarang)
                {
                    await UIDialogue.RunDialogue(Scenes[4]);
                    TaskSystem.Main.Run(() => TutorialBeli.SetActive(true));
                    ClickBukuTugas = new TaskCompletionSource<bool>();
                    await ClickBukuTugas.Task;
                    TaskSystem.Main.Run(() => TutorialBeli.SetActive(false));
                    await UIDialogue.RunDialogue(Scenes[5]);
                    BeliBarang = new TaskCompletionSource<bool>();
                    await BeliBarang.Task;
                    await UIDialogue.RunDialogue(Scenes[6]);
                    DataSystem.GameData.Tutorial.FinishBeliBarang();
                    DataSystem.SaveGame();
                }
                if (DataSystem.GameData.Bunny.IsSick)
                {
                    await UISickModal.Sick();
                    DataSystem.SaveGame();
                }
            });
        }

        public void StartMinigame()
        {
            if (DataSystem.GameData.Player.GetResource("Energy") >= 10)
            {
                DataSystem.GameData.Player.AddResource("Energy", -10);
                ClickMainCariBarang.TrySetResult(true);
                GameEnd = new TaskCompletionSource<bool>();
                TaskSystem.Main.Run(() =>
                {
                    GameUI.SetActive(false);
                    MinigameUI.SetActive(true);
                });
            }
        }

        public void EndMinigame()
        {
            GameEnd.TrySetResult(true);
            DataSystem.GameData.Player.AddResource("Heart", 1);
            // TODO: Get coin
            //DataSystem.GameData.Player.AddResource("Coin", 1);
            DataSystem.SaveGame();
            TaskSystem.Main.Run(() =>
            {
                GameUI.SetActive(true);
                MinigameUI.SetActive(false);
            });
        }

        public void NewGame()
        {
            DataSystem.NewGame();
        }

        public void ToggleMusic(bool val)
        {
            if (val)
            {
                DataSystem.SettingData.SetMusic(1);
            }
            else
            {
                DataSystem.SettingData.SetMusic(0);
            }
            DataSystem.SaveSettings();
        }

        public void ToggleSound(bool val)
        {
            if (val)
            {
                DataSystem.SettingData.SetSound(1);
            }
            else
            {
                DataSystem.SettingData.SetSound(0);
            }
            DataSystem.SaveSettings();
        }

        public void BukuTugas()
        {
            ClickBukuTugas.TrySetResult(true);
        }

        private void purchaseEvent()
        {
            BeliBarang.TrySetResult(true);
        }
    }
}
