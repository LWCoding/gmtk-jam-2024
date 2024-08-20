using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuyableSlot : SlotHandler
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _costText;

    private TileObject _tileObject;
    
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
        if (canAfford == _isDraggable) { return; }  // Ignore if state is same as before
        // Or else, change the state of this slot
        if (canAfford)
        {
            _isDraggable = true;
        } else
        {
            _isDraggable = false;
        }
        _costText.color = _isDraggable ? Color.green : Color.red;
    }

    public override void RenderLogicAt(Vector3 worldPos)
    {
        // If there's already something here, don't do anything
        if (TilemapManager.Instance.GetTileAtPosition(worldPos) != null) { return; }
        AudioManager.Instance.PlayOneShot(SFX.BUILD_OBJECT); 
        GameManager.Money -= _tileObject.CostToBuy;  // Spend money
        TilemapManager.Instance.PlaceTileAt(worldPos, _tileObject);  // Place tile
    }
}
