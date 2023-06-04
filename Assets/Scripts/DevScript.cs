using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.UI;
using UnityEngine;

/// <summary>
/// DEV ONLY
/// </summary>
public class DevScript : MonoBehaviour
{
    [SerializeField] GameObject toggle;
    [SerializeField] GameObject buttons;
    [SerializeField] HouseMainThread houseMainThread;
    [SerializeField] UITugas uiTugas;
    [SerializeField] ResourceManager resourceManager;
#if DEVELOPMENT_BUILD
    private void Awake()
    {
        toggle.SetActive(true);
    }
#endif

    public void ToggleDev()
    {
        buttons.SetActive(!buttons.activeSelf);
    }

    public void TimeSkip()
    {
        resourceManager.TimeSkip(3 * 24 * 60 * 60 * 1000);
        _ = houseMainThread.ToggleBunnySick();
    }

    public void UnlockAll()
    {
        uiTugas.UnlockAll();
    }
}
