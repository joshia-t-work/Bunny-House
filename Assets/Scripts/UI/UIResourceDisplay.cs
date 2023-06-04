using BunnyHouse.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays a resource as a text
/// </summary>
public class UIResourceDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    [SerializeField]
    private Resource resource;
    /// <summary>
    /// If true, renders with <c>value/maxValue</c>
    /// </summary>
    [SerializeField]
    private bool showMax = true;
    /// <summary>
    /// If resource is a time, rendered in <c>mm:ss</c>
    /// </summary>
    [SerializeField]
    private bool isTime = false;

    private void Update()
    {
        if (showMax && resource.HasMaxValue)
        {
            label.text = getValue() + $"/{resource.MaxValue}";
        }
        else
        {
            label.text = getValue();
        }
    }

    private string getValue()
    {
        if (isTime)
        {
            TimeSpan time = TimeSpan.FromSeconds(resource.Get());
            return time.ToString(@"m\:ss");
        } else
        {
            return resource.Get().ToString();
        }
    }
}
