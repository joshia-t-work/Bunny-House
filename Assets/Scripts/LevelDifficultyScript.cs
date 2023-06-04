using BunnyHouse.Data.Events;
using BunnyHouse.Data.Scene;
using UnityEngine;
using System.Collections.Generic;
using BunnyHouse.Core;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Represents level controller during minigame, enable to apply selected difficulty
/// </summary>
public class LevelDifficultyScript : MonoBehaviour
{
    [SerializeField] SearchItemScript searchItemPrefab;
    [SerializeField] GlobalEvent difficultyEvent;
    [SerializeField] GlobalEvent UsedHintEvent;
    [SerializeField] GlobalEventSO itemEnabled;
    [SerializeField] GlobalEventSO itemClicked;
    [SerializeField] SearchItemList _searchItems;
    [SerializeField] TextAsset positions;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] List<SearchItemScript> _items = new List<SearchItemScript>();
    [SerializeField] ImageDisplayScript[] _displays;
    List<SearchItem> itemsToFind = new List<SearchItem>();
    Dictionary<SearchItem, Coroutine> hintedItems = new();
    private void Awake()
    {
        difficultyEvent.AddListener(difficultyEventListener);
        gameObject.SetActive(false);
        UsedHintEvent.AddListener(usedHintEvent);
    }

    private void OnEnable()
    {
        itemClicked.AddListener(onItemClick);
        itemsToFind.Clear();
        foreach (var item in hintedItems)
        {
            StopCoroutine(item.Value);
        }
        hintedItems.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            itemsToFind.Add(_items[i].item);
        }
        StartCoroutine(DelayedOnEnable());
    }
    IEnumerator DelayedOnEnable()
    {
        yield return new WaitForEndOfFrame();
        Refresh();
    }
    private void OnDisable()
    {
        itemClicked.RemoveListener(onItemClick);
    }
    private void OnDestroy()
    {
        difficultyEvent.RemoveListener(difficultyEventListener);
        UsedHintEvent.RemoveListener(usedHintEvent);
    }

    private void difficultyEventListener()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void Refresh()
    {
        for (int i = 0; i < _displays.Length; i++)
        {
            if (itemsToFind.Count > i)
            {
                itemEnabled.Invoke(itemsToFind[i]);
                _displays[i].Set(itemsToFind[i]);
            } else
            {
                _displays[i].Clear();
            }
        }
    }

    private void usedHintEvent()
    {
        if (itemsToFind.Count > 0)
        {
            int tryHint = 0;
            SearchItem searchItem = null;
            while (tryHint < 10)
            {
                tryHint++;
                int rand = Random.Range(0, Mathf.Min(_displays.Length, itemsToFind.Count));
                searchItem = itemsToFind[rand];
                if (!hintedItems.ContainsKey(searchItem))
                {
                    tryHint = 999;
                }
            }
            if (!hintedItems.ContainsKey(searchItem))
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].item == searchItem)
                    {
                        hintedItems.Add(searchItem, StartCoroutine(jitter(_items[i])));
                    }
                }
            }
        }
    }

    private IEnumerator jitter(SearchItemScript item)
    {
        Vector3 pos = item.transform.position;
        float time = 0;
        while (time < 0.2f)
        {
            Vector3 rand = Random.insideUnitCircle * 0.2f;
            item.transform.position = pos + rand;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        item.transform.position = pos;
        yield return new WaitForSeconds(2f);
        yield return jitter(item);
    }

    /// <summary>
    /// Remove clicked item from search list
    /// </summary>
    private void onItemClick(ScriptableObject so)
    {
        SearchItem item = (SearchItem)so;

        // stop hint
        if (hintedItems.ContainsKey(item))
        {
            StopCoroutine(hintedItems[item]);
            hintedItems.Remove(item);
        }

        if (itemsToFind.Contains(item))
        {
            itemsToFind.Remove(item);
        }
        Refresh();
        if (itemsToFind.Count == 0)
        {
            StartCoroutine(delayedEnd());
        }
    }

    /// <summary>
    /// Delays end to allow animation to finish
    /// </summary>
    IEnumerator delayedEnd()
    {
        DataSystem.GameData.Player.AddResource("Time", 2f);
        yield return new WaitForSeconds(1f);
        DataSystem.SaveGame();
        HouseMainThread.I.EndMinigame();
    }
    #region Load Python OpenCV generated data to locate each objects' screen location (done externally)
#if UNITY_EDITOR
    [ContextMenu("Generate Items")]
    private void generateItems()
    {
        _items.Clear();
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            DestroyImmediate(transform.GetChild(childs - 1 - i).gameObject);
        }
        float width = GetComponent<SpriteRenderer>().sprite.rect.width;
        float height = GetComponent<SpriteRenderer>().sprite.rect.height;
        float ppu = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        string[] items = positions.text.Split("\n");
        for (int i = 0; i < items.Length; i++)
        {
            string[] itemData = items[i].Split("|");
            if (itemData.Length > 2)
            {
                string itemName = itemData[0];
                Position topLeftPosition = getPosition(itemData[1].Trim());
                Position botRightPosition = getPosition(itemData[2].Trim());
                ItemPosition itemPosition = new ItemPosition(
                    topLeftPosition.ScaledCenter(width, height, ppu),
                    botRightPosition.ScaledCenter(width, height, ppu)
                    );
                SearchItemScript searchItem = (SearchItemScript)PrefabUtility.InstantiatePrefab(searchItemPrefab);
                _items.Add(searchItem);
                searchItem.name = itemName.ToUpper();
                searchItem.transform.position = transform.position + itemPosition.centerOff;
                searchItem.transform.parent = transform;
                for (int ii = 0; ii < _searchItems.Items.Length; ii++)
                {
                    if (_searchItems.Items[ii].ItemName.ToLower() == itemName.ToLower())
                    {
                        searchItem.item = _searchItems.Items[ii];
                    }
                }
                for (int ii = 0; ii < _sprites.Length; ii++)
                {
                    if (_sprites[ii].name.ToLower() == itemName.ToLower())
                    {
                        searchItem.GetComponent<SpriteRenderer>().sprite = _sprites[ii];
                    }
                }
                searchItem.gameObject.AddComponent<PolygonCollider2D>();
            }
        }
    }
#endif

    [System.Serializable]
    internal class Position
    {
        public float x;
        public float y;
        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return $"({x}, {y})";
        }
        public Position ScaledCenter(float width, float height, float ppu)
        {
            x -= 0.5f;
            y -= 0.5f;
            x *= width;
            y *= -height;
            x /= ppu;
            y /= ppu;
            return this;
        }
    }

    [System.Serializable]
    internal class ItemPosition
    {
        public Position tl;
        public Position br;
        public Vector3 centerOff;
        public Vector3 size;
        public ItemPosition(Position tl, Position br)
        {
            this.tl = tl;
            this.br = br;
            Vector3 tlv = new Vector3(tl.x, tl.y);
            Vector3 brv = new Vector3(br.x, br.y);
            centerOff = (tlv + brv) / 2f;
            size = tlv - brv;
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y));
        }
    }

    private Position getPosition(string posString)
    {
        string[] positions = posString.Substring(1, posString.Length - 2).Split(",");
        return new Position(float.Parse(positions[0].Trim()), float.Parse(positions[1].Trim()));
    }
    #endregion
}
