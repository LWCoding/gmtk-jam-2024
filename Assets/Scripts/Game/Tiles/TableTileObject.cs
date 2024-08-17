using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Table Tile", menuName = "Tiles/Table Tile")]
public class TableTile : TileObject
{
    public override void Interact()
    {
        Debug.Log("Interacted with table");
    }

}
