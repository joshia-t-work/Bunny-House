using BunnyHouse.Core;
using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchItemScript : MonoBehaviour
{
    public SearchItem item;
    bool disappeared = false;
    bool canClick = false;
    SpriteRenderer sr;
    [SerializeField] GlobalEventSO itemEnabled;
    [SerializeField] GlobalEventSO itemClicked;
    [SerializeField] AudioClip clickedSound;
    [SerializeField] AudioClip wrongSound;

    private void OnEnable()
    {
        disappeared = false;
        sr.enabled = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        canClick = false;
        itemEnabled.AddListener(onItemEnable);
    }

    private void OnDisable()
    {
        itemEnabled.RemoveListener(onItemEnable);
    }

    private void onItemEnable(ScriptableObject so)
    {
        SearchItem item = (SearchItem)so;
        if (item == this.item)
        {
            canClick = true;
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (!Singleton.isUIOverride())
        {
            if (!disappeared)
            {
                if (canClick)
                {
                    SoundSystem.PlaySound(clickedSound);
                    disappeared = true;
                    itemClicked.Invoke(item);
                    if (gameObject.activeSelf)
                    {
                        StartCoroutine(disappear());
                    }
                } else
                {
                    SoundSystem.PlaySound(wrongSound);
                }
            }
        }

    }

    IEnumerator disappear()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - t);
            yield return null;
        }
        sr.enabled = false;
    }
}
