using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Table Tile", menuName = "Tiles/Table Tile")]
public class TableTile : TileObject
{

    public List<Vector3> PossibleSeatPositions = new();
    public List<Quaternion> PossibleSeatRotations = new();
    private List<CustomerHandler> SeatedPeople = new();

    public override void Initialize(Vector3Int position)
    {
        PossibleSeatPositions = new();
        PossibleSeatRotations = new();
        SeatedPeople = new();
        var tileObjects = TilemapManager.Instance.TileObjects;
        Vector3Int[] offsets = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };
        foreach (Vector3Int offset in offsets)
        {
            // If there is something NOT adjacent to this side of table
            Vector3Int adjPosition = position + offset;
            if (TilemapManager.Instance.IsValidTile(adjPosition) && !tileObjects.ContainsKey(adjPosition))
            {
                // Calculate the angle in degrees
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

                PossibleSeatPositions.Add(adjPosition + new Vector3(0.5f, 0.5f));
                PossibleSeatRotations.Add(Quaternion.Euler(0, 0, angle));
            }
        }
    }

    public bool HasSeats()
    {
        return SeatedPeople.Count < PossibleSeatPositions.Count;
    }

    public void SitPerson(CustomerHandler customerHandler)
    {
        GameObject personObject = customerHandler.gameObject;
        personObject.transform.position = PossibleSeatPositions[SeatedPeople.Count];
        personObject.transform.localRotation = PossibleSeatRotations[SeatedPeople.Count];
        personObject.transform.Rotate(0, 0, 90);
        SeatedPeople.Add(customerHandler);
    }

    public override void Interact(Vector3Int position)
    {
        Player.Instance.DropItem();  // Call EatServeDfood on SEatedPoeple instead
    }

    public override void OnEnterInteractRange(Vector3Int position)
    {
        IsInteractable = Player.Instance.IsHoldingItem();
    }

    public override void OnExitInteractRange(Vector3Int position)
    {
       
    }

}
