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
    /// Moves an object at a specified position, and stores it into the
    /// dictionary at a specified ID.
    /// </summary>
    public void MovePopupThen(GameObject obj, Vector3 position, float popupDuration, Action action)
    {
        StartCoroutine(SpawnPopupThenCoroutine(obj, position, popupDuration, action));
    }

    private IEnumerator SpawnPopupThenCoroutine(GameObject obj, Vector3 position, float popupDuration, Action action)
    {
        obj.transform.position = position;
        yield return new WaitForSeconds(popupDuration);
        Destroy(obj);
        action?.Invoke();
    }

}
