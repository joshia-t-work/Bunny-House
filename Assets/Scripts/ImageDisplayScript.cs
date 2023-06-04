using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents an image, fitted by ratio and a description
/// </summary>
public class ImageDisplayScript : MonoBehaviour
{
    public AspectRatioFitter fitter;
    public Image img;
    public TMP_Text text;
    public void Clear()
    {
        img.color = Color.clear;
        text.text = "";
    }
    public void Set(SearchItem item)
    {
        img.color = Color.white;
        img.sprite = item.Image;
        text.text = item.ItemName;
        fitter.aspectRatio = item.Image.rect.width / item.Image.rect.height;
    }
}
