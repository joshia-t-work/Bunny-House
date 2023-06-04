using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSettingsUI : MonoBehaviour
{
    [SerializeField] AudioClip ClickSound;
    [SerializeField] Toggle Sound;
    [SerializeField] Toggle Music;
    int stupidUnityButton = 0;
    private void Awake()
    {
        Sound.SetIsOnWithoutNotify(DataSystem.SettingData.Sound != 1);
        Music.SetIsOnWithoutNotify(DataSystem.SettingData.Music != 1);
    }
    private void OnEnable()
    {
        Sound.SetIsOnWithoutNotify(DataSystem.SettingData.Sound != 1);
        Music.SetIsOnWithoutNotify(DataSystem.SettingData.Music != 1);
    }
    private void Update()
    {
        stupidUnityButton += 1;
    }
    public void UIOnValueSound(bool val)
    {
        if (stupidUnityButton > 10)
        {
            SoundSystem.PlaySound(ClickSound);
            if (val)
            {
                DataSystem.SettingData.SetSound(0);
            }
            else
            {
                DataSystem.SettingData.SetSound(1);
            }
            DataSystem.SaveSettings();
        }
    }
    public void UIOnValueMusic(bool val)
    {
        if (stupidUnityButton > 10)
        {
            SoundSystem.PlaySound(ClickSound);
            if (val)
            {
                DataSystem.SettingData.SetMusic(0);
            }
            else
            {
                DataSystem.SettingData.SetMusic(1);
            }
            DataSystem.SaveSettings();
        }
    }
}
