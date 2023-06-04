using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM handler
/// </summary>
/// <remarks>Singleton</remarks>
public class BGMScript : MonoBehaviour
{
    [SerializeField]
    ModifiedSource source1 = new();
    [SerializeField]
    ModifiedSource source2 = new();
    Coroutine current;
    enum Direction
    {
        ToSource1,
        ToSource2
    }
    Direction currentDirection = Direction.ToSource1;
    Dictionary<AudioClip, float> clipTimes = new();
    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length == 2)
        {
            source1.Source = sources[0];
            source2.Source = sources[1];
            source1.Volume = 0;
        } else
        {
            Debug.LogError("Unexpected audiosource count!");
        }
    }
    private void Update()
    {
        source1.Source.volume = DataSystem.SettingData.Music * source1.Volume;
        source2.Source.volume = DataSystem.SettingData.Music * source2.Volume;
    }

    /// <summary>
    /// Switches channel according to currently on channel
    /// </summary>
    /// <remarks>When called during transition, transition progress is preserved and reversed</remarks>
    public void PlayBGM(AudioClip clip)
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        switch (currentDirection)
        {
            case Direction.ToSource1:
                currentDirection = Direction.ToSource2;
                current = StartCoroutine(SwitchBGM(clip, source2, source1));
                break;
            case Direction.ToSource2:
                currentDirection = Direction.ToSource1;
                current = StartCoroutine(SwitchBGM(clip, source1, source2));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Fades volume from source to target in 1 second
    /// </summary>
    IEnumerator SwitchBGM(AudioClip clip, ModifiedSource from, ModifiedSource target)
    {
        float time = target.Volume;
        target.Source.clip = clip;
        target.Source.Play();
        if (clipTimes.TryGetValue(clip, out float newTime))
        {
            target.Source.time = newTime;
        }
        while (time < 1f)
        {
            time += Time.deltaTime;
            from.Volume = Mathf.Min(from.Volume, 1f - time);
            target.Volume = Mathf.Max(target.Volume, time);
            yield return null;
        }
        if (from.Source.isPlaying)
        {
            if (clipTimes.ContainsKey(from.Source.clip))
            {
                clipTimes[from.Source.clip] = from.Source.time;
            } else
            {
                clipTimes.Add(from.Source.clip, from.Source.time);
            }
        }
        from.Source.Stop();
        current = null;
    }

    /// <summary>
    /// AudioSource with internal volume set from 0 to 1, not affected by sound settings
    /// </summary>
    [System.Serializable]
    class ModifiedSource
    {
        public AudioSource Source;
        public float Volume = 1f;
    }
}
