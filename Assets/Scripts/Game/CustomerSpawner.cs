using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{

    [Header("Spawner Properties")]
    [SerializeField] private CustomerHandler _customerPrefab;
    [SerializeField] private float _delayBetweenSpawns;
    [SerializeField] private Transform _spawnLocationTransform;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 3, _delayBetweenSpawns);
    }

    /// <summary>
    /// Spawn an instance of a customer at the spawner's start location.
    /// Make them face downwards.
    /// </summary>
    public void SpawnCustomer()
    {
        Instantiate(_customerPrefab, _spawnLocationTransform.position, Quaternion.Euler(0, 0, 180));
    }

}
