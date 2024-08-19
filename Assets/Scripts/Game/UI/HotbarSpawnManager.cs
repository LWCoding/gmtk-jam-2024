using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSpawnManager : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private BuyableSlot _buyableSlotPrefab;
    [SerializeField] private SellSlot _sellableSlotPrefab;
    [SerializeField] private Transform _slotParent;

    private void Start()  // Doesn't work if Awake for some reason :(
    {
        SlotHandler slot;
        // Get all buyable slots, place them in the hotbar
        foreach (TileObject tile in GameManager.AllTileObjects)
        {
            if (!tile.IsPurchaseable) { return; }
            slot = Instantiate(_buyableSlotPrefab, _slotParent);
            slot.Initialize(tile);
        }
        // Place a sellable slot at the end of the hotbar
        slot = Instantiate(_sellableSlotPrefab, _slotParent);
        slot.Initialize(null);  // Sellable slot doesn't need tile info
    }

}
