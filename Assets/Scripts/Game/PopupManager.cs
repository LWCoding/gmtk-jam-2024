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
