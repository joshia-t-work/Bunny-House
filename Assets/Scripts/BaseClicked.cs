using UnityEngine;

/// <summary>
/// Used for minigame backgrounds to detect clicks
/// </summary>
public class BaseClicked : MonoBehaviour
{
    [SerializeField]
    AudioClip IncorrectSound;
    public void OnMouseDown()
    {
        SoundSystem.PlaySound(IncorrectSound, 2f);
    }
}
