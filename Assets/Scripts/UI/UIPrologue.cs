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
    public class UIPrologue : MonoBehaviour
    {
        [SerializeField] Image Image;
        [SerializeField] TMP_Text Text;
        public async Task RunPrologue(DialogueScene Scene)
        {
            TaskSystem.Main.Run(() => gameObject.SetActive(true));
            for (int i = 0; i < Scene.Dialogues.Count; i++)
            {
                await TaskSystem.Run(async () =>
                {
                    string imgname = Scene.Dialogues[i].DisplayName;
                    Sprite sprite = null;
                    for (int ii = 0; ii < Scene.Images.Count; ii++)
                    {
                        if (Scene.Images[ii].FileName == imgname)
                        {
                            sprite = Scene.Images[ii].Image;
                            break;
                        }
                    }
                    if (sprite != null)
                    {
                        Image.sprite = sprite;
                    }
                    Text.text = Scene.Dialogues[i].DialogueContent.Replace("(nama)", DataSystem.GameData.Player.Name).Replace("(nama kelinci)", DataSystem.GameData.Bunny.Name);
                    await Task.Delay(5000);
                });
            }
            TaskSystem.Main.Run(() => gameObject.SetActive(false));
        }
    }
}