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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(uiMousePos);
        _image.transform.position = uiMousePos;
        TilemapManager.Instance.EraseTileAt(mousePos);  // Erase tile
    }

    public override void Initialize(TileObject tileObj)
    {

    }

    public override void RenderLogicAt(Vector3 pos)
    {
        TilemapManager.Instance.EraseTileAt(pos);  // Erase tile
    }

}
