using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{

    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;

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

    [Header("BG Tilemap")]
    [SerializeField] private Tilemap _bgTilemap;
    [SerializeField] private TileBase _floorTile;
    [Header("Obstacle Tilemap")]
    [SerializeField] private Tilemap _obstacleTilemap;

    private readonly Dictionary<Vector3Int, TileObject> _tileObjects = new();
    [SerializeField, ReadOnly] private TileObject _currSelectedTileObject = null;
    [SerializeField, ReadOnly] private TileObject _currActiveTileObject = null;
    [SerializeField, ReadOnly] private List<TileObject> _allTileObjects = new();
    [SerializeField, ReadOnly] private List<Vector3Int> _ghostTilePositions = new();

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
        PlaceTileAt(new(-2, -2, 0), _allTileObjects[0]);
        PlaceTileAt(new(1, 1, 0), _allTileObjects[1]);
        // Draw all floor tiles
        DrawBGTiles();
    }

    private void Update()
    {
        CheckForMouseClick();
        CheckForMouseDrag();
        CheckForMouseUp();
    }

    private void DrawBGTiles()
    {
        for (int i = GameManager.Instance.TilemapLeftLimit; i < GameManager.Instance.TilemapRightLimit; i++)
        {
            for (int j = GameManager.Instance.TilemapDownLimit; j < GameManager.Instance.TilemapUpLimit; j++)
            {
                _bgTilemap.SetTile(new(i, j, 0), _floorTile);
            }
        }
    }

    public bool IsValidTile(Vector3Int pos)
    {
        return pos.x >= GameManager.Instance.TilemapLeftLimit && pos.x < GameManager.Instance.TilemapRightLimit &&
               pos.y >= GameManager.Instance.TilemapDownLimit && pos.y < GameManager.Instance.TilemapUpLimit;
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
    /// Given a position and a tile to place at that position, places the tile
    /// in ghost mode, adding it to the _ghostTileObjects list.
    /// 
    /// IF the tile is already occupied, does not make a ghost tile.
    /// </summary>
    public void CreateGhostTileAt(Vector3Int position, TileObject tileObject)
    {
        if (_obstacleTilemap.GetTile(position) != null) { return; }

        _obstacleTilemap.SetTile(position, tileObject.Tile);
        _obstacleTilemap.SetColor(position, new Color(1, 1, 1, 0.4f));
        _ghostTilePositions.Add(position);
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
    /// Erases all ghost tiles that have been added up to this point.
    /// </summary>
    public void EraseAllGhostTiles()
    {
        foreach (Vector3Int pos in _ghostTilePositions)
        {
            _obstacleTilemap.SetTile(pos, null);
        }
        _ghostTilePositions.Clear();
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
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down);

        if (!IsValidTile(tpos)) { return; }  // If we've hit a non-valid tile, stop

        // If we've clicked on a different layer mask, deselect current object
        if (_currActiveTileObject != null && (!hit || !IsInLayerMask(hit.collider.gameObject, _currActiveTileObject.AllowedLayer))) {
            _currActiveTileObject.Uninteract(tpos);
            _currActiveTileObject = null;
        }

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
            _obstacleTilemap.SetColor(_originalClickedPosition, new Color(1, 1, 1));
            if (!_tileObjects.TryGetValue(tpos, out TileObject selectedObj) && IsValidTile(tpos))
            {
                EraseTileAt(_originalClickedPosition);
                PlaceTileAt(tpos, _currSelectedTileObject);
            }
        } 
        else
        {
            _currActiveTileObject = _currSelectedTileObject;
            _currSelectedTileObject.Interact(tpos);
        }

        _isDragging = false;
        _currSelectedTileObject = null;
    }

}
