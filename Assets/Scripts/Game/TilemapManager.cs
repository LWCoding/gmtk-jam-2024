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

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        CheckForMouseClick();
    }

    /// <summary>
    /// When the mouse is clicked on an obstacle on the tilemap,
    /// renders logic related to that obstacle.
    /// </summary>
    public void CheckForMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) { return; }
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down);

        if (hit.collider != null)
        {
            Debug.Log("click on " + hit.collider.name);
            Debug.Log(hit.point);

            Vector3Int tpos = _obstacleTilemap.WorldToCell(hit.point);

            // Try to get a tile from cell position
            TileBase tile = _obstacleTilemap.GetTile(tpos);
        }
    }

    // TODO: Check scriptable tile example:
    // https://docs.unity3d.com/Manual/Tilemap-ScriptableTiles-Example.html

}
