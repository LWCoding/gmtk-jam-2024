using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Table Tile", menuName = "Tiles/Table Tile")]
public class TableTile : TileObject
{

    [ReadOnly] public List<Vector3> PossibleSeatPositions = new();

    public override void Initialize(Vector3Int position)
    {
        PossibleSeatPositions = new();
        var tileObjects = TilemapManager.Instance.TileObjects;
        Vector3Int[] offsets = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };
        foreach (Vector3Int offset in offsets)
        {
            // If there is something NOT adjacent to this side of table
            Vector3Int adjPosition = position + offset;
            if (TilemapManager.Instance.IsValidTile(adjPosition) && !tileObjects.ContainsKey(adjPosition))
            {
                PossibleSeatPositions.Add(adjPosition + new Vector3(0.5f, 0.5f));
                new GameObject("Seat").transform.position = adjPosition + new Vector3(0.5f, 0.5f);
            }
        }
    }

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
