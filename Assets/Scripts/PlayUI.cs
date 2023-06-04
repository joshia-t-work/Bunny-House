using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayUI : MonoBehaviour
{
    readonly string[] DIFF = new string[]{
        "Mudah",
        "Normal",
        "Sulit",
    };
    [SerializeField] TMP_Text diffText;
    private void OnEnable()
    {
        diffText.text = DIFF[DataSystem.GameData.Player.Difficulty % DIFF.Length];
    }
}
