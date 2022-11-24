using BunnyHouse.Core;
using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    public class UIDialogue : MonoBehaviour
    {
        [SerializeField] TMP_Text Name;
        [SerializeField] TMP_Text Text;
        private TaskCompletionSource<bool> NextClicked = new TaskCompletionSource<bool>();
        public async Task RunDialogue(DialogueScene Scene)
        {
            TaskSystem.Main.Run(() => gameObject.SetActive(true));
            for (int i = 0; i < Scene.Dialogues.Count; i++)
            {
                await TaskSystem.Run(async () =>
                {
                    Name.text = Scene.Dialogues[i].DisplayName;
                    Text.text = Scene.Dialogues[i].DialogueContent.Replace("(nama)", DataSystem.GameData.Player.Name).Replace("(nama kelinci)", DataSystem.GameData.Bunny.Name);
                    NextClicked = new TaskCompletionSource<bool>();
                    await NextClicked.Task;
                });
            }
            TaskSystem.Main.Run(() => gameObject.SetActive(false));
        }
        public void Next()
        {
            NextClicked.TrySetResult(true);
        }
    }
}