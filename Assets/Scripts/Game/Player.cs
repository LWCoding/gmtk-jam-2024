using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    private GameObject _holding = null;

    [Header("Object Assignments")]
    [SerializeField] private Transform _playerStartTransform;

    public bool IsHoldingItem() => _holding != null;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        transform.position = _playerStartTransform.position;
    }

    /// <summary>
    /// Makes the player start holding an item.
    /// It is recommended you check IsHoldingItem() before executing this function.
    /// </summary>    
    public void HoldItem(GameObject obj)
    {
        _holding = obj;
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
    }

    /// <summary>
    /// Make the player stop holding whatever they're holding.
    /// Destroys the object they are holding.
    /// </summary>
    public void DropItem()
    {
        Destroy(_holding);
    }

}
