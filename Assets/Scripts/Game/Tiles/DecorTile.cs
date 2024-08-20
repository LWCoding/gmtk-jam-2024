using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Decor Tile", menuName = "Tiles/Decor Tile")]
public class DecorTile : TileObject
{

    [Header("Decor Boosts")]
    public float DecorBuff;

    public override void Initialize(Vector3Int position)
    {
        GameManager.Instance.DecorBuff += DecorBuff;
    }

    public override void Interact(Vector3Int position)
    {

    }

    public override void OnEnterInteractRange(Vector3Int position)
    {

    }

    public override void OnExitInteractRange(Vector3Int position)
    {

    }

}
