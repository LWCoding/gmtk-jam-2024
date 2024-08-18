using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TilemapInteractManager : TilemapManager
{

    private readonly List<Vector3Int> _allCloseInteractables = new();

    private const float INTERACT_DISTANCE = 1.5f;

    private void Update()
    {
        ActivateTilesNearPlayer();
        DeactivateTilesFarFromPlayer();
    }

    /// <summary>
    /// Checks the proximity around the player and shows if any
    /// tiles nearby are interactable.
    /// </summary>
    private void ActivateTilesNearPlayer()
    {
        Vector3 playerPos = Player.Instance.transform.position;

        foreach (Vector3Int pos in _tileObjects.Keys)
        {
            if (_allCloseInteractables.Contains(pos)) { return; }
            if (Vector3.Distance(playerPos, (Vector3)pos + new Vector3(0.5f, 0.5f, 0f)) < INTERACT_DISTANCE)
            {
                _obstacleTilemap.SetColor(pos, Color.yellow);
                _allCloseInteractables.Add(pos);
            }
        }
    }

    private void DeactivateTilesFarFromPlayer()
    {
        Vector3 playerPos = Player.Instance.transform.position;

        List<Vector3Int> tooFarAway = _allCloseInteractables.FindAll((pos) => Vector3.Distance(playerPos, (Vector3)pos + new Vector3(0.5f, 0.5f, 0)) >= INTERACT_DISTANCE);

        for (int i = tooFarAway.Count - 1; i >= 0; i--)
        {
            _obstacleTilemap.SetColor(tooFarAway[i], Color.white);
            _allCloseInteractables.RemoveAt(i);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // Draw interaction range for interactable tiles
        foreach (Vector3Int pos in _tileObjects.Keys)
        {
            Gizmos.DrawWireSphere(pos + new Vector3(0.5f, 0.5f, 0f), 1.5f);
        }
    }
#endif

}
