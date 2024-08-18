using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
public class CustomerHandler : MonoBehaviour
{

    private AIDestinationSetter _aiSetter;

    private void Awake()
    {
        _aiSetter = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 180);  // Start facing down
        StartCoroutine(WaitAndStartTracking());
    }

    private IEnumerator WaitAndStartTracking()
    {
        yield return new WaitForSeconds(1);
        _aiSetter.target = Player.Instance.transform;
    }

}
