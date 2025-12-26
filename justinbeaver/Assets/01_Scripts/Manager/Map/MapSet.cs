using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Maps/MapSet", fileName = "MapSet")]
public class MapSet : ScriptableObject
{
    public List<GameObject> mapPrefabs;
}
