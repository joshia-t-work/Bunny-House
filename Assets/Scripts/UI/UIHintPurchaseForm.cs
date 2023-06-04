using BunnyHouse.Core;
using BunnyHouse.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents Hint Purchasing Form
/// </summary>
public class UIHintPurchaseForm : MonoBehaviour
{
    [SerializeField] TMP_Text hintPurchaseCount;
    [SerializeField] TMP_Text hintPurchaseCost;
    int purchaseCount = 1;
    const int purchaseCost = 300;
    Color defaultColor;

    private void Awake()
    {
        defaultColor = hintPurchaseCost.color;
    }

    private void OnEnable()
    {
        purchaseCount = 1;
        RefreshDisplay();
    }

    /// <summary>
    /// Refreshes and recolor the text based on cost and player money
    /// </summary>
    private void RefreshDisplay()
    {
        hintPurchaseCount.text = $"{purchaseCount}";
        if (purchaseCount * purchaseCost > DataSystem.GameData.Player.GetResource("Coin"))
        {
            hintPurchaseCost.color = Color.red;
        } else
        {
            hintPurchaseCost.color = defaultColor;
        }
        hintPurchaseCost.text = $"{purchaseCount * purchaseCost}";
    }

    /// <remarks>For UI calls</remarks>
    public void Add1Hint()
    {
        purchaseCount += 1;
        RefreshDisplay();
    }

    /// <remarks>For UI calls</remarks>
    public void Remove1Hint()
    {
        purchaseCount = (int)Mathf.Max(purchaseCount - 1, 1f);
        RefreshDisplay();
    }

    /// <remarks>For UI calls</remarks>
    public void Buy()
    {
        if (DataSystem.GameData.Player.GetResource("Coin") >= purchaseCount * purchaseCost)
        {
            DataSystem.GameData.Player.AddResource("Coin", -purchaseCount * purchaseCost);
            DataSystem.GameData.Player.AddResource("Hint", purchaseCount);
            gameObject.SetActive(false);
        }
    }
}
