using UnityEngine;

/// <summary>
/// Represents level difficulties
/// </summary>
public class Levels : MonoBehaviour
{
    [SerializeField]
    LevelDifficultyScript[] levelDifficultyScripts;
    /// <summary>
    /// Reactivates the active difficulty to reset level state
    /// </summary>
    public void ResetLevel()
    {
        for (int i = 0; i < levelDifficultyScripts.Length; i++)
        {
            if (levelDifficultyScripts[i].isActiveAndEnabled)
            {
                levelDifficultyScripts[i].gameObject.SetActive(false);
                levelDifficultyScripts[i].gameObject.SetActive(true);
            }
        }
    }
}
