using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Tooltip Assignments")]
    [SerializeField] protected GameObject _tooltipParent;
    [SerializeField] protected TextMeshProUGUI _tooltipText;

    public abstract void Initialize(TileObject obj);

    private void Start()
    {
        _tooltipParent.SetActive(false);  // Hide parent tooltip at start
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
