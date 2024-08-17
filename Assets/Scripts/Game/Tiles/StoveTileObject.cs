using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stove Tile", menuName = "Tiles/Stove Tile")]
public class StoveTile : TileObject
{
    public override void Interact()
    {
        Debug.Log("Interacted with stove");
    }

}
