using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PanHandler : MonoBehaviour
{

    [Header("Sprite Properties")]
    [SerializeField] private Sprite _cookedSprite;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.Play("Cook");
    }

    /// <summary>
    /// Call to stop the cooking animation and make the pan look like its
    /// done cooking.
    /// </summary>
    public void FinishCooking()
    {
        _animator.enabled = false;
        _spriteRenderer.sprite = _cookedSprite;
    }

}
