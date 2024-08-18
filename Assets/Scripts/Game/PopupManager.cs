using System;
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

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    private readonly Dictionary<string, GameObject> _popups = new();

    /// Spawns an object at a specified position, and stores it into the
    /// dictionary at a specified ID. Can be deleted with `DeleteObject`.
    /// </summary>
    public void SpawnObject(GameObject obj, Vector3 position, string id)
    {
        GameObject copy = Instantiate(obj, position, Quaternion.identity);
        _popups[id] = copy;
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

    /// <summary>
    /// Spawns an object at a specified position, and stores it into the
    /// dictionary at a specified ID. Can be deleted with `DeleteObject`.
    /// </summary>
    public void SpawnPopupThen(GameObject obj, Vector3 position, float popupDuration, Action action)
    {
        StartCoroutine(SpawnPopupThenCoroutine(obj, position, popupDuration, action));
    }

    private IEnumerator SpawnPopupThenCoroutine(GameObject obj, Vector3 position, float popupDuration, Action action)
    {
        GameObject newObj = Instantiate(obj, position, Quaternion.identity);
        yield return new WaitForSeconds(popupDuration);
        Destroy(newObj);
        action?.Invoke();
    }

}
