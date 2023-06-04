using BunnyHouse.Data;
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
    /// <summary>
    /// Main game controller
    /// </summary>
    public class HouseMainThread : MonoBehaviour
    {
        public static HouseMainThread I;
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
        [SerializeField] GameObject TutorialActuallyBeli;
        [SerializeField] GameObject TutorialDock;
        [SerializeField] GameObject TutorialHint;
        [SerializeField] GlobalEventSO PurchaseEvent;
        [SerializeField] GlobalEvent[] Difficulties;
        [SerializeField] GlobalEvent UseHintEvent;
        [SerializeField] GlobalEvent UsedHintEvent;
        [SerializeField] GameObject BuyHintUI;
        [Header("Music")]
        [SerializeField] AudioClip HouseBGM;
        [SerializeField] AudioClip GudangBGM;
        private ResourceManager rm;
        private TaskCompletionSource<bool> GameStart = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> GameEnd = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickMainCariBarang = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickBukuTugas = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickDock = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> ClickHint = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> BeliBarang = new TaskCompletionSource<bool>();
        public static long ticksPassed;

        private void Awake()
        {
            rm = GetComponent<ResourceManager>();
            I = this;
            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;
            ticksPassed = ticks - DataSystem.GameData.LastTicksOpen;
            if (ticksPassed < 0)
            {
                ticksPassed = 0;
            }
            if (ticksPassed / 10000000 / 60 / 60 / 24 > 3)
            {
                DataSystem.GameData.Bunny.SetSick(true);
            }
            PurchaseEvent.AddListener(purchaseEvent);
            UseHintEvent.AddListener(useHintEvent);
        }

        private void OnDestroy()
        {
            PurchaseEvent.RemoveListener(purchaseEvent);
            UseHintEvent.RemoveListener(useHintEvent);
            TaskSystem.StopAll();
        }

        /// <summary>
        /// Main timeline of game, flow controlled by async Tasks that are awaited by Tutorial successions
        /// </summary>
        void Start()
        {
            GameStart = new TaskCompletionSource<bool>();
            SoundSystem.PlayBGM(HouseBGM);
            UIDescModal.Initialize();
            _ = TaskSystem.Run(async () =>
            {
                if (DataSystem.GameData.Player.Name == "")
                {
                    string pName = await UIPopup.ShowPrompt("Siapa Namamu?");
                    if (pName == "")
                    {
                        pName = " ";
                    }
                    DataSystem.GameData.Player.SetName(pName);
                    await UIPrologue.RunPrologue(Scenes[0]);
                    DataSystem.SaveGame();
                }
                if (DataSystem.GameData.Bunny.Name == "")
                {
                    await UIDialogue.RunDialogue(Scenes[1]);
                    string kName = await UIPopup.ShowPrompt("Siapa Nama Kelincimu?");
                    if (kName == "")
                    {
                        kName = " ";
                    }
                    DataSystem.GameData.Bunny.SetName(kName);
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
                    await GameStart.Task;
                    await GameEnd.Task;
                    DataSystem.GameData.Tutorial.FinishCariBarang();
                    DataSystem.SaveGame();
                }
                if (!DataSystem.GameData.Tutorial.BeliBarang)
                {
                    rm.PlayerDataSet("Heart", 1);
                    TaskSystem.Main.Run(() => TutorialBeli.SetActive(true));
                    ClickBukuTugas = new TaskCompletionSource<bool>();
                    await ClickBukuTugas.Task;
                    TaskSystem.Main.Run(() => TutorialBeli.SetActive(false));
                    TaskSystem.Main.Run(() => TutorialActuallyBeli.SetActive(true));
                    BeliBarang = new TaskCompletionSource<bool>();
                    await BeliBarang.Task;
                    TaskSystem.Main.Run(() => TutorialActuallyBeli.SetActive(false));
                    await UIDialogue.RunDialogue(Scenes[3]);
                    DataSystem.GameData.Tutorial.FinishBeliBarang();
                    DataSystem.SaveGame();
                }
                if (DataSystem.GameData.Bunny.IsSick)
                {
                    await ToggleBunnySick();
                }
            });
        }

        /// <summary>
        /// Opens bunny sick modal, waits for return and saves data
        /// </summary>
        public async Task ToggleBunnySick()
        {
            await UISickModal.Sick();
            DataSystem.SaveGame();
        }

        /// <summary>
        /// Starts the minigame, waits for minigame to end
        /// </summary>
        public async void StartMinigame()
        {
            if (DataSystem.GameData.Player.GetResource("Energy") >= 10)
            {
                GameEnd = new TaskCompletionSource<bool>();
                GameStart.TrySetResult(true);
                SoundSystem.PlayBGM(GudangBGM);
                DataSystem.GameData.Player.AddResource("Energy", -10);
                ClickMainCariBarang.TrySetResult(true);
                Difficulties[DataSystem.GameData.Player.Difficulty % Difficulties.Length].Invoke();
                TaskSystem.Main.Run(() =>
                {
                    GameUI.SetActive(false);
                    MinigameUI.SetActive(true);
                });
                if (!DataSystem.GameData.Tutorial.Dock)
                {
                    TaskSystem.Main.Run(() => TutorialDock.SetActive(true));
                    ClickDock = new TaskCompletionSource<bool>();
                    await ClickDock.Task;
                    TaskSystem.Main.Run(() => TutorialDock.SetActive(false));
                    DataSystem.GameData.Tutorial.FinishDock();
                    DataSystem.SaveGame();
                }
                if (!DataSystem.GameData.Tutorial.Hint)
                {
                    TaskSystem.Main.Run(() => TutorialHint.SetActive(true));
                    ClickHint = new TaskCompletionSource<bool>();
                    await ClickHint.Task;
                    TaskSystem.Main.Run(() => TutorialHint.SetActive(false));
                    DataSystem.GameData.Tutorial.FinishHint();
                    DataSystem.SaveGame();
                }
                await GameEnd.Task;
                SoundSystem.PlayBGM(HouseBGM);
            }
        }

        /// <summary>
        /// Called to end minigame, both prematurely and as intended
        /// </summary>
        /// <remarks>Also used for UI calls</remarks>
        public void EndMinigame()
        {
            GameStart = new TaskCompletionSource<bool>();
            Difficulties[DataSystem.GameData.Player.Difficulty % Difficulties.Length].Invoke();
            GameEnd.TrySetResult(true);
            if (rm.PlayerDataGet("Time") > 0)
            {
                DataSystem.GameData.Player.Difficulty += 1;
                rm.PlayerDataAdd("Heart", 1);
                rm.PlayerDataAdd("Coin", rm.PlayerDataGet("Time") + 150);
            }
            DataSystem.SaveGame();
            TaskSystem.Main.Run(() =>
            {
                GameUI.SetActive(true);
                MinigameUI.SetActive(false);
            });
        }

        /// <remarks>For UI calls</remarks>
        public void NewGame()
        {
            DataSystem.NewGame();
        }

        /// <remarks>For UI calls</remarks>
        public void ToggleMusic(bool val)
        {
            if (val)
            {
                DataSystem.SettingData.SetMusic(0);
            }
            else
            {
                DataSystem.SettingData.SetMusic(1);
            }
            DataSystem.SaveSettings();
        }

        /// <remarks>For UI calls</remarks>
        public void ToggleSound(bool val)
        {
            if (val)
            {
                DataSystem.SettingData.SetSound(0);
            }
            else
            {
                DataSystem.SettingData.SetSound(1);
            }
            DataSystem.SaveSettings();
        }

        /// <remarks>For UI calls</remarks>
        public void BukuTugas()
        {
            ClickBukuTugas.TrySetResult(true);
        }

        /// <remarks>For UI calls</remarks>
        public void Dock()
        {
            ClickDock.TrySetResult(true);
        }

        /// <remarks>For UI calls</remarks>
        public void Hint()
        {
            ClickHint.TrySetResult(true);
        }

        private void purchaseEvent(ScriptableObject so)
        {
            BeliBarang.TrySetResult(true);
        }

        private void useHintEvent()
        {
            if (DataSystem.GameData.Player.GetResource("Hint") > 0)
            {
                rm.PlayerDataAdd("Hint", -1f);
                UsedHintEvent.Invoke();
            } else
            {
                BuyHintUI.SetActive(true);
            }
        }

        public void UIRestartMinigame()
        {
            if (DataSystem.GameData.Player.GetResource("Energy") >= 10)
            {
                DataSystem.GameData.Player.AddResource("Energy", -10);
                MinigameUI.SetActive(false);
                MinigameUI.SetActive(true);
            }
        }
    }
}
