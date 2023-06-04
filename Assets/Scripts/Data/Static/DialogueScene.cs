using System;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Scene
{
    /// <summary>
    /// Represents a dialogue scene containing images or text spoken by a character (either dialogue or prologue scene)
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue Scene", menuName = "SO/Dialogue Scene")]
    public class DialogueScene : ScriptableObject
    {
        public List<Dialogue> Dialogues = new List<Dialogue>();
        public List<DialogueImage> Images = new List<DialogueImage>();
    }

    /// <summary>
    /// Represents the core content of the dialogue
    /// </summary>
    [Serializable]
    public class Dialogue
    {
        /// <summary>
        /// When on dialogue scene, DisplayName is used as the character name, on prologue, DisplayName is used to refer to the DialogueImage FileName
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// Content text of the dialogue
        /// </summary>
        [TextArea]
        public string DialogueContent;
    }

    /// <summary>
    /// Represents an image with a label for prologue usage
    /// </summary>
    [Serializable]
    public class DialogueImage
    {
        /// <summary>
        /// is used to be refered by prologue DisplayName
        /// </summary>
        public string FileName;
        public Sprite Image;
    }
}