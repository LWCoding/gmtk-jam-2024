using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class CustomerHandler : MonoBehaviour
{

    private AIPath _aiPath;
    private AIDestinationSetter _aiSetter;
    private TableTile _trackedTable;
    private GameObject _createdTablePos;

    private Transform _doorTransform;

    private int _ticketNumber;

    private const float DIST_TO_TABLE_BEFORE_SITTING = 0.8f;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
        _aiSetter = GetComponent<AIDestinationSetter>();
        _doorTransform = GameObject.FindGameObjectWithTag("Door").transform;
    }

    private void Start()
    {
        StartCoroutine(WaitAndStartTracking());
    }

    private IEnumerator WaitAndStartTracking()
    {
        yield return new WaitForSeconds(1);
        FindTable();
        // If we couldn't find a target
        if (_aiSetter.target == null)
        {
            // Turn around and leave
            float currTime = 0;
            float timeToWait = 0.8f;
            while (currTime < timeToWait)
            {
                currTime += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 0), currTime / timeToWait);
                yield return null;
            }
            Destroy(gameObject);  // Destroy myself (leave)
        } 
        else
        {
            // Or else, walk towards table until nearby
            StartCoroutine(WaitUntilNearTable());
        }
    }   

    /// <summary>
    /// Locate a table to navigate to using the TileManager and set the target 
    /// to that table. If no seats are found, keeps the target as null.
    /// </summary>
    private void FindTable()
    {
        foreach (Vector3Int key in TilemapManager.Instance.TileObjects.Keys)
        {
            TileObject tileObj = TilemapManager.Instance.TileObjects[key];
            if (tileObj is TableTile tile)
            {
                if (!tile.HasSeats())  // If no seats, keep looking
                {
                    continue;
                }
                GameObject tablePos = new("Table Position");  // Create temp object to track to
                tablePos.transform.position = key + new Vector3(0.5f, 0.5f);
                _aiSetter.target = tablePos.transform;
                _trackedTable = tile;
                _createdTablePos = tablePos;
                return;
            }
        }
    }

    private IEnumerator WaitUntilNearTable()
    {
        yield return new WaitUntil(() => Vector2.Distance(transform.position, _aiSetter.target.position) < DIST_TO_TABLE_BEFORE_SITTING);
        
        // If the table we were tracking is taken up, find another table
        if (!_trackedTable.HasSeats())
        {
            FindTable();
        }
        
        _aiPath.enabled = false;  // Temporarily disable AI
        _ticketNumber = _trackedTable.SitPerson(this);
        Destroy(_createdTablePos);  // Delete temp object
    }

    public void EatServedFood()
    {
        StartCoroutine(EatServedFoodCoroutine());
    }

    private IEnumerator EatServedFoodCoroutine()
    {
        yield return new WaitForSeconds(1);

        _aiSetter.target = _doorTransform;
        _aiPath.enabled = true;  // Make this go back to the door before destroying

        _trackedTable.RemovePerson(_ticketNumber);

        yield return new WaitUntil(() => Vector2.Distance(transform.position, _aiSetter.target.position) < DIST_TO_TABLE_BEFORE_SITTING);
        Destroy(gameObject);
    }

}
