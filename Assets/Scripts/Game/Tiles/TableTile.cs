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
    public List<CustomerHandler> SeatedPeople = new();

    public List<int> AvailableSeats = new();

    public override void Initialize(Vector3Int position)
    {
        PossibleSeatPositions = new();
        PossibleSeatRotations = new();
        AvailableSeats = new();
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

                AvailableSeats.Add(PossibleSeatPositions.Count);
                PossibleSeatPositions.Add(adjPosition + new Vector3(0.5f, 0.5f));
                PossibleSeatRotations.Add(Quaternion.Euler(0, 0, angle));
            }
        }
    }

    public bool HasSeats()
    {
        return SeatedPeople.Count < PossibleSeatPositions.Count;
    }

    public int SitPerson(CustomerHandler customerHandler)
    {
        GameObject personObject = customerHandler.gameObject;
        int availableSeatIdx = AvailableSeats[0];
        AvailableSeats.RemoveAt(0);
        personObject.transform.position = PossibleSeatPositions[availableSeatIdx];
        personObject.transform.localRotation = PossibleSeatRotations[availableSeatIdx];
        personObject.transform.position -= personObject.transform.right * 0.35f;
        personObject.transform.Rotate(0, 0, 90);
        SeatedPeople.Add(customerHandler);
        return availableSeatIdx;
    }

    public override void Interact(Vector3Int position)
    {
        if (SeatedPeople.Count == 0) { return; }  // Only interactable with people seated
        SeatedPeople[0].EatServedFood();
        SeatedPeople.RemoveAt(0);
        Player.Instance.DropItem();
    }

    public void RemoveLatestPerson()
    {
        SeatedPeople.RemoveAt(0);
    }

    public void RemovePerson(int availableSeatIdx)
    {
        AvailableSeats.Add(availableSeatIdx);
    }

    public override void OnEnterInteractRange(Vector3Int position)
    {
        IsInteractable = SeatedPeople.Count > 0 && Player.Instance.IsHoldingItem();
    }

    public override void OnExitInteractRange(Vector3Int position)
    {
       
    }

}
