using BunnyHouse.Core;
using UnityEngine;

/// <summary>
/// Singleton script to manage sound
/// </summary>
public class SoundSystem : MonoSingleton
{
    static SoundManager soundManager;
    public override void MonoAwake()
    {
        soundManager = GetComponentInChildren<SoundManager>();
    }

    public static void PlaySound(AudioClip clip, float volume = 1f)
    {
        soundManager.PlaySound(clip, volume);
    }

    public static void PlayBGM(AudioClip clip)
    {
        soundManager.PlayBGM(clip);
    }
}
