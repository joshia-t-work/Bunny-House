using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BunnyHouse.UI
{
    public class UIPopup : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text Prompt;
        [SerializeField]
        private TMP_InputField Input;
        private TaskCompletionSource<string> OnPromptSubmit = new TaskCompletionSource<string>();

        private string curVal;

        public void OnChangeText(string text)
        {
            curVal = text;
        }
        public void Submit()
        {
            TaskSystem.Main.Run(() => gameObject.SetActive(false));
            OnPromptSubmit.TrySetResult(curVal);
        }
        public Task<string> ShowPrompt(string text)
        {
            curVal = "";
            Input.text = "";
            Prompt.text = text;
            TaskSystem.Main.Run(() => gameObject.SetActive(true));
            OnPromptSubmit = new TaskCompletionSource<string>();
            return OnPromptSubmit.Task;
        }
    }
}
