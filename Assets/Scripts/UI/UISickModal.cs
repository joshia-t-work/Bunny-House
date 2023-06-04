using BunnyHouse.Core;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace BunnyHouse.UI
{
    public class UISickModal : MonoBehaviour
    {
        private TaskCompletionSource<bool> OnPromptSubmit = new TaskCompletionSource<bool>();
        /// <remarks>For UI calls</remarks>
        public void Submit()
        {
            TaskSystem.Main.Run(() => gameObject.SetActive(false));
            OnPromptSubmit.TrySetResult(true);
            float cost = 100;
            try
            {
                DataSystem.GameData.Player.SetResource("Coin", DataSystem.GameData.Player.GetResource("Coin") - cost);
                BunnyScript.I.isHidden = false;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        /// <summary>
        /// Shows the sick modal, waits until the modal is submitted
        public Task Sick()
        {
            BunnyScript.I.isHidden = true;
            DataSystem.GameData.Bunny.SetSick(false);
            TaskSystem.Main.Run(() => gameObject.SetActive(true));
            OnPromptSubmit = new TaskCompletionSource<bool>();
            return OnPromptSubmit.Task;
        }
    }
}
