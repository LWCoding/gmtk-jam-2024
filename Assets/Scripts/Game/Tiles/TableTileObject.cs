using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Table Tile", menuName = "Tiles/Table Tile")]
public class TableTile : TileObject
{

    public override void Interact(Vector3Int position)
    {
        Player.Instance.DropItem();
    }

    public override void OnEnterInteractRange(Vector3Int position)
    {
        IsInteractable = Player.Instance.IsHoldingItem();
    }

    public override void OnExitInteractRange(Vector3Int position)
    {
       
    }

}
