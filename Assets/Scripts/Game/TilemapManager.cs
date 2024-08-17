using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{

    private static TilemapManager _instance;
    public static TilemapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TilemapManager>();
            }
            return _instance;
        }
    }

    [Header("Obstacle Tilemap")]
    [SerializeField] private Tilemap _obstacleTilemap;

    private readonly Dictionary<Vector3Int, TileObject> _tileObjects = new();
    [SerializeField, ReadOnly] private TileObject _currSelectedTileObject = null;
    [SerializeField, ReadOnly] private List<TileObject> _allTileObjects = new();

    private Vector3Int _originalClickedPosition;  // Position that the mouse is clicked
    private bool _isDragging = false;  // Only set to true if mouse is moved a distance after clicking

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        // Find all tile objects from Resources folder
        _allTileObjects = new(Resources.LoadAll<TileObject>("Special Tiles"));
        // Place a few tiles at the beginning of the game
        PlaceTileAt(new(0, 0, 0), _allTileObjects[0]);
        PlaceTileAt(new(-1, -1, 0), _allTileObjects[0]);
    }

    private void Update()
    {
        CheckForMouseClick();
        CheckForMouseDrag();
        CheckForMouseUp();
    }

    /// <summary>
    /// Given a position and a tile to place at that position, places the tile.
    /// </summary>
    public void PlaceTileAt(Vector3Int position, TileObject tileObject)
    {
        _obstacleTilemap.SetTile(position, tileObject.Tile);
        _tileObjects[position] = tileObject;
    }

    /// <summary>
    /// Given a position, erases the tile at that position if one exists.
    /// </summary>
    public void EraseTileAt(Vector3Int position)
    {
        _obstacleTilemap.SetTile(position, null);
        _tileObjects.Remove(position);
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
        if (!_isDragging && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), (Vector3)_originalClickedPosition) > 0.5f)
        {
            _isDragging = true;
        }

        // Render logic if we are dragging.
        if (_isDragging)
        {
            
        } else
        {
            
        }
    }

    /// <summary>
    /// When the mouse is let go AND we're currently selecting an
    /// obstacle on the tilemap, renders let-go logic.
    /// </summary>
    private void CheckForMouseUp()
    {
        if (!Input.GetMouseButtonUp(0) || _currSelectedTileObject == null) { return; }

        if (_isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tpos = _obstacleTilemap.WorldToCell(mousePos);

            if (_tileObjects.TryGetValue(tpos, out TileObject selectedObj))
            {
                Debug.Log("cannot place here");
            }
            else
            {
                EraseTileAt(_originalClickedPosition);
                PlaceTileAt(tpos, _currSelectedTileObject);
            }
        } else
        {
            _currSelectedTileObject.Interact();
        }

        _currSelectedTileObject = null;
    }

}
