using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MyTile", menuName = "ScriptableObjects/TileData", order = 1)]
public class TileData : ScriptableObject
{
    public Sprite tileImage;
    public int tileType;

}
