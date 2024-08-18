using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{

    private static PopupManager _instance;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();
            }
            return _instance;
        }
    }

    private Dictionary<string, GameObject> _popups = new();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Spawns an object at a specified position, and stores it into the
    /// dictionary at a specified ID. Can be deleted with `DeleteObject`.
    /// </summary>
    public void SpawnObject(GameObject obj, Vector3 position, string id)
    {
        GameObject newObj = Instantiate(obj, position, Quaternion.identity);
        _popups[id] = newObj;
    }
    
    /// <summary>
    /// If an object with the specified ID exists, deletes that object.
    /// </summary>
    public void DeleteObject(string id)
    {
        if (_popups.ContainsKey(id))
        {
            Destroy(_popups[id]);
            _popups.Remove(id);
        }
    }

}
