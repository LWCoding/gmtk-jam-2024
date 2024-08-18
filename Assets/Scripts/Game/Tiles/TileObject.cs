using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileObject : ScriptableObject
{

    [Header("Base Tile Properties")]
    public TileBase Tile;
    public LayerMask AllowedLayer;

    public abstract void Interact(Vector3Int position);

    public abstract void Uninteract(Vector3Int position);

}
