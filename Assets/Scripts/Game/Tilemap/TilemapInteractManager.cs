using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TilemapInteractManager : TilemapManager
{

    private List<Vector3Int> _allCloseInteractables = new();

    private const float INTERACT_DISTANCE = 1.5f;

    private void Update()
    {
        ActivateTilesNearPlayer();
        DeactivateTilesFarFromPlayer();
        CheckPlayerInteraction();
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
            if (Vector3.Distance(playerPos, (Vector3)pos + new Vector3(0.5f, 0.5f, 0f)) < INTERACT_DISTANCE)
            {
                _tileObjects[pos].OnEnterInteractRange(pos);

                if (_allCloseInteractables.Contains(pos) || !_tileObjects[pos].IsInteractable) { continue; }

                _obstacleTilemap.SetColor(pos, Color.yellow);
                _allCloseInteractables.Add(pos);
            }
        }
    }

    private void DeactivateTilesFarFromPlayer()
    {
        Vector3 playerPos = Player.Instance.transform.position;

        List<Vector3Int> shouldDeactivate = _allCloseInteractables.FindAll((pos) => !_tileObjects[pos].IsInteractable || Vector3.Distance(playerPos, (Vector3)pos + new Vector3(0.5f, 0.5f, 0)) >= INTERACT_DISTANCE);

        for (int i = shouldDeactivate.Count - 1; i >= 0; i--)
        {
            Vector3Int pos = shouldDeactivate[i];
            _tileObjects[pos].OnExitInteractRange(pos);
            _obstacleTilemap.SetColor(pos, Color.white);
            _allCloseInteractables.RemoveAt(i);
        }
    }

    /// <summary>
    /// Check if the interact key is pressed, and call Interact() on the
    /// closest interactable object if applicable.
    /// </summary>
    private void CheckPlayerInteraction()
    {
        Vector3 playerPos = Player.Instance.transform.position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Screen out all non-interactable objects in list, in case one was *just* disabled!
            _allCloseInteractables = _allCloseInteractables.FindAll((pos) => _tileObjects[pos].IsInteractable);

            if (_allCloseInteractables.Count == 0) { return; }  // If no interactables, stop
            
            // First, get the closest interactable tile position
            Vector3Int closest = _allCloseInteractables[0];
            float closestDist = 999;
            for (int i = 1; i < _allCloseInteractables.Count; i++)
            {
                Vector3 currPos = (Vector3)_allCloseInteractables[i] + new Vector3(0.5f, 0.5f, 0);
                float currDist = Vector3.Distance(playerPos, currPos);
                if (currDist < closestDist)
                {
                    closest = _allCloseInteractables[i];
                    closestDist = currDist;
                }
            }

            _obstacleTilemap.SetColor(closest, Color.white);
            _tileObjects[closest].Interact(closest);
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
