using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stove Tile", menuName = "Tiles/Stove Tile")]
public class StoveTile : TileObject
{

    [Header("Object Assignments")]
    [SerializeField] private GameObject _stoveUIPrefab;

    private Vector3 _popupOffset = new(0, 1.5f);

    public override void Interact(Vector3Int position)
    {
        PopupManager.Instance.SpawnObject(_stoveUIPrefab, (Vector3)position + new Vector3(0.5f, 0.5f, 0) + _popupOffset, "stove");
    }

    public override void Uninteract(Vector3Int position)
    {
        PopupManager.Instance.DeleteObject("stove");
    }

}
