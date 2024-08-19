using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlotHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [Header("Object Assignments")]
    [SerializeField] private RectTransform _imageObject;

    private bool _isDragging = false;

    private void Update()
    {
        if (!_isDragging) { return; }
        _imageObject.transform.position = Input.mousePosition;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        // Get tile at position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (TilemapManager.Instance.GetTileAtPosition(mousePos) == null)
        {
            Debug.Log("PLACED!");
        } 
        else
        {
            Debug.Log("COULD NOT PLACE.");
        }
        _isDragging = false;
        _imageObject.transform.localPosition = Vector3.zero; // Reset to original position
    }

}
