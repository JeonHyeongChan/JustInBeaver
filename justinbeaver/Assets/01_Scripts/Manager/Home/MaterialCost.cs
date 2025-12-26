using System;
using UnityEngine;

[Serializable]
public struct MaterialCost
{
    [Header("Material")]
    public ItemData item;   // Wood, Stone, Metal, Glass

    [Header("Amount")]
    public int amount;
}
