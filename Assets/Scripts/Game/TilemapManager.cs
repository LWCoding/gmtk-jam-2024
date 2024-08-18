using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TilemapManager : MonoBehaviour
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

    [Header("BG Tilemap")]
    [SerializeField] protected Tilemap _bgTilemap;
    [SerializeField] protected TileBase _floorTile;
    [Header("Obstacle Tilemap")]
    [SerializeField] protected Tilemap _obstacleTilemap;

    protected readonly Dictionary<Vector3Int, TileObject> _tileObjects = new();
    [SerializeField, ReadOnly] protected TileObject _currSelectedTileObject = null;
    protected List<TileObject> _allTileObjects = new();
    protected List<Vector3Int> _ghostTilePositions = new();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        // Find all tile objects from Resources folder
        _allTileObjects = new(Resources.LoadAll<TileObject>("Special Tiles"));
        // Place a few tiles at the beginning of the game   (  TODO:   REMOVE THIS   !!   )
        PlaceTileAt(new(-2, -2, 0), _allTileObjects[0]);
        PlaceTileAt(new(1, 1, 0), _allTileObjects[1]);
        // Draw all floor tiles
        DrawBGTiles();
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

}
