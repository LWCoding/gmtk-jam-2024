using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class SlotHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Tooltip Assignments")]
    [SerializeField] protected GameObject _tooltipParent;
    [SerializeField] protected TextMeshProUGUI _tooltipText;
    [Header("Object Assignments")]
    [SerializeField] protected Image _image;

    protected bool _isDraggable = true;
    protected bool _isDragging;

    /// <summary>
    /// Initialize the slot's information. `obj` may be null if no slot
    /// information is required.
    /// </summary>
    public abstract void Initialize(TileObject obj);

    /// <summary>
    /// Given a position, render the logic when this slot is released
    /// on that position.
    /// </summary>
    public abstract void RenderLogicAt(Vector3 pos);

    private void Start()
    {
        _tooltipParent.SetActive(false);  // Hide parent tooltip at start
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!_isDraggable) { return; }  // If we're not draggable, don't do anything
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!_isDragging) { return; }  // If we're not dragging, don't do anything
        // Get tile at position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TilemapManager.Instance.EraseAllGhostTiles();  // Erase ghost tiles to make room
        if (TilemapManager.Instance.GetTileAtPosition(mousePos) == null)
        {
            RenderLogicAt(mousePos);
        }
        _isDragging = false;
        _image.transform.localPosition = Vector3.zero; // Reset to original position
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltipParent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltipParent.SetActive(false);
    }

}
