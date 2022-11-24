using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    [CreateAssetMenu(fileName = "Dialogue Scene", menuName = "SO/Dialogue Scene")]
    public class DialogueScene : ScriptableObject
    {
        public List<Dialogue> Dialogues = new List<Dialogue>();
        public List<DialogueImage> Images = new List<DialogueImage>();
    }

    [Serializable]
    public class DialogueImage
    {
        public string FileName;
        public Sprite Image;
    }

    [Serializable]
    public class Dialogue
    {
        public string DisplayName;
        [TextArea]
        public string DialogueContent;
    }
}