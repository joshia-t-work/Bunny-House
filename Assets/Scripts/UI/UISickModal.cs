using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BunnyHouse.UI
{
    public class UISickModal : MonoBehaviour
    {
        private TaskCompletionSource<bool> OnPromptSubmit = new TaskCompletionSource<bool>();
        public void Submit()
        {
            TaskSystem.Main.Run(() => gameObject.SetActive(false));
            OnPromptSubmit.TrySetResult(true);
        }
        public Task Sick()
        {
            // TODO: ADD COST TO PAYING SICK
            DataSystem.GameData.Bunny.SetSick(false);
            TaskSystem.Main.Run(() => gameObject.SetActive(true));
            OnPromptSubmit = new TaskCompletionSource<bool>();
            return OnPromptSubmit.Task;
        }
    }
}
