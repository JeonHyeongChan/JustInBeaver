using System;
using UnityEngine;

[Serializable]
public struct MaterialCost
{
    [Header("재료")]
    public ItemData item;   //Wood, Stone, Metal, Glass

    [Header("수량")]
    public int amount;
}
