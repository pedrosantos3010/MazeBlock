using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Tile Set", menuName = "Tile Set", order = 1)]
public class TileSet : ScriptableObject
{
    public Transform[] prefabs = new Transform[0];
}
