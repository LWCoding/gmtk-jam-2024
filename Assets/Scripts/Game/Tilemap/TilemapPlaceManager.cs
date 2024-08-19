using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPlaceManager : TilemapManager
{

    private Vector3Int _originalClickedPosition;  // Position that the mouse is clicked
    private bool _isDragging = false;  // Only set to true if mouse is moved a distance after clicking

    public TileObject CurrentSelectedTile
    {
        get => _currSelectedTileObject;
    }
    [SerializeField, ReadOnly] protected TileObject _currSelectedTileObject = null;

    private void Update()
    {
        CheckForMouseClick();
        CheckForMouseDrag();
        CheckForMouseUp();
    }

    /// <summary>
    /// Save the tiles to the static class when this scene is changed.
    /// </summary>
    public void OnDestroy()
    {
        GameManager.RestaurantTiles = _tileObjects;
    }

    /// <summary>
    /// When the mouse is clicked on an obstacle on the tilemap,
    /// renders logic related to that obstacle.
    /// </summary>
    private void CheckForMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) { return; }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tpos = _obstacleTilemap.WorldToCell(mousePos);

        if (!IsValidTile(tpos)) { return; }  // If we've hit a non-valid tile, stop

        _originalClickedPosition = tpos;
        _isDragging = false;

        // If we find an obstacle here, select it!
        if (_tileObjects.TryGetValue(tpos, out TileObject selectedObj))
        {
            _currSelectedTileObject = selectedObj;
        }
    }

    /// <summary>
    /// When the mouse is dragged AND we're currently selecting an
    /// obstacle on the tilemap, renders drag logic.
    /// </summary>
    private void CheckForMouseDrag()
    {
        if (!Input.GetMouseButton(0) || _currSelectedTileObject == null) { return; }

        // If we've moved the mouse far enough from the orig position, we are dragging. Called ONCE
        if (!_isDragging && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), ((Vector3)_originalClickedPosition) + new Vector3(0.5f, 0.5f, 0)) > 0.5f)
        {
            _isDragging = true;
            _obstacleTilemap.SetColor(_originalClickedPosition, new Color(0.6f, 0.6f, 0.6f));
        }

        // Render logic if we are dragging.
        if (_isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tpos = _obstacleTilemap.WorldToCell(mousePos);

            if (!IsValidTile(tpos)) { return; }

            EraseAllGhostTiles();
            CreateGhostTileAt(tpos, _currSelectedTileObject);
        }
    }

    /// <summary>
    /// When the mouse is let go AND we're currently selecting an
    /// obstacle on the tilemap, renders let-go logic.
    /// </summary>
    private void CheckForMouseUp()
    {
        if (!Input.GetMouseButtonUp(0) || _currSelectedTileObject == null) { return; }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tpos = _obstacleTilemap.WorldToCell(mousePos);

        if (_isDragging)
        {
            EraseAllGhostTiles();
            _obstacleTilemap.SetColor(_originalClickedPosition, Color.white);
            if (!_tileObjects.TryGetValue(tpos, out TileObject selectedObj) && IsValidTile(tpos))
            {
                EraseTileAt(_originalClickedPosition);
                PlaceTileAt(tpos, _currSelectedTileObject);
            }
        } 

        _isDragging = false;
        _currSelectedTileObject = null;
    }

}
