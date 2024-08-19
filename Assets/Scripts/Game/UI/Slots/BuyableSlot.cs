using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuyableSlot : SlotHandler, IPointerDownHandler, IPointerUpHandler
{

    [Header("Object Assignments")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _costText;

    private TileObject _tileObject;
    private bool _isDragging = false;
    private bool _isPurchaseable = false;  // Used to ignore repeat calls to `EnableBuyIfRich`

    /// <summary>
    /// When we're dragging, move this sprite's image object to the
    /// mouse position. Also spawn ghost tiles at positions.
    /// </summary>
    private void Update()
    {
        if (!_isDragging) { return; }
        Vector3 uiMousePos = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(uiMousePos);
        _image.transform.position = uiMousePos;
        TilemapManager.Instance.EraseAllGhostTiles();
        TilemapManager.Instance.CreateGhostTileAt(mousePos, _tileObject);  // Place tile
    }

    /// <summary>
    /// Cache this tile's information to render logic when placed.
    /// Also subscribe to events.
    /// </summary>
    public override void Initialize(TileObject tileObj)
    {
        _tileObject = tileObj;
        _image.sprite = tileObj.Tile.sprite;
        _costText.text = "$" + tileObj.CostToBuy.ToString();
        _tooltipText.text = tileObj.ShopName + ":\n" + tileObj.ShopDescription;
        ModifyStateBasedOnMoney(GameManager.Money);
    }

    private void OnEnable()
    {
        GameManager.OnMoneyChanged += ModifyStateBasedOnMoney;
    }

    private void OnDisable()
    {
        GameManager.OnMoneyChanged -= ModifyStateBasedOnMoney;
    }

    /// <summary>
    /// Make this slot interactable based on available money and
    /// the cost of this slot.
    /// </summary>
    public void ModifyStateBasedOnMoney(int currMoney)
    {
        bool canAfford = currMoney >= _tileObject.CostToBuy;
        if (canAfford == _isPurchaseable) { return; }  // Ignore if state is same as before
        // Or else, change the state of this slot
        if (canAfford)
        {
            _isPurchaseable = true;
        } else
        {
            _isPurchaseable = false;
        }
        _costText.color = _isPurchaseable ? Color.green : Color.red;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!_isPurchaseable) { return; }  // If we're not purchaseable, don't do anything
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
            GameManager.Money -= _tileObject.CostToBuy;  // Spend money
            TilemapManager.Instance.PlaceTileAt(mousePos, _tileObject);  // Place tile
        }
        _isDragging = false;
        _image.transform.localPosition = Vector3.zero; // Reset to original position
    }

}
