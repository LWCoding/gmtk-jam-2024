using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileObject : ScriptableObject
{

    [Header("Base Tile Properties")]
    public TileBase Tile;
    public bool IsInteractable = false;

    public abstract void Interact(Vector3Int position);

}
