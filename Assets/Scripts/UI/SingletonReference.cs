using BunnyHouse.Core;
using UnityEngine;

namespace BunnyHouse.UI
{
    /// <summary>
    /// Represents a reference that can be referred to in a Scene to call methods on persistent (singleton) objects
    /// </summary>
    public class SingletonReference : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneSystem.LoadScene(sceneName);
        }
        public void PlaySound(AudioClip clip)
        {
            SoundSystem.PlaySound(clip);
        }
    }
}