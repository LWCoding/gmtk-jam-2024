using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : SlotHandler
{

    /// <summary>
    /// When we're dragging, move this sprite's image object to the
    /// mouse position. Also spawn ghost tiles at positions.
    /// </summary>
    private void Update()
    {
        if (!_isDragging) { return; }
        Vector3 uiMousePos = Input.mousePosition;
        _image.transform.position = uiMousePos;
    }

    public override void Initialize(TileObject tileObj)
    {

    }

    public override void RenderLogicAt(Vector3 worldPos)
    {
        // If there's nothing here, don't do anything
        if (TilemapManager.Instance.GetTileAtPosition(worldPos) == null) { return; }
        TileObject tileAtPosition = TilemapManager.Instance.GetTileAtPosition(worldPos);
        if (tileAtPosition == null) { return; }
        GameManager.Money += tileAtPosition.CostToBuy / 2;
        TilemapManager.Instance.EraseTileAt(worldPos);  // Erase tile
    }

}
