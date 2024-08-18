using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stove Tile", menuName = "Tiles/Stove Tile")]
public class StoveTile : TileObject
{

    [Header("Object Assignments")]
    [SerializeField] private GameObject _stoveUIPrefab;
    [SerializeField] private GameObject _stovePanPrefab;

    private Vector3 _popupOffset = new(0, 1.5f);

    private GameObject _panObject = null;  // If no pan on stove, null, else GameObject

    public override void Interact(Vector3Int position)
    {
        // If we have a pan, interact with the pan instead!
        if (_panObject != null)
        {
            Destroy(_panObject);
            _panObject = null;
            return;
        }

        // Interact with the stove and cook some food
        IsInteractable = false;
        Vector3 stovePosition = (Vector3)position + new Vector3(0.5f, 0.5f, 0);

        // Create a pan to show at the stove
        _panObject = Instantiate(_stovePanPrefab, stovePosition, Quaternion.identity);

        // Spawn the popup above the stove
        PopupManager.Instance.SpawnPopupThen(_stoveUIPrefab, stovePosition + _popupOffset, 8, () =>
        {
            IsInteractable = true;
        });
    }

}
