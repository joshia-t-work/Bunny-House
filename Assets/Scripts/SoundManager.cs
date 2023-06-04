using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages sound volumes according to setting
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource SoundPlayer;
    [SerializeField] BGMScript BGM;

    private void Update()
    {
        SoundPlayer.volume = DataSystem.SettingData.Sound;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        SoundPlayer.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip)
    {
        BGM.PlayBGM(clip);
    }
}
