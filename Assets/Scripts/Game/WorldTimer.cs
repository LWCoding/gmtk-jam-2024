using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldTimer : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private SpriteRenderer _fillRenderer;
    [Header("Shader Assignment")]
    [SerializeField] private Material _radialFillMaterial;

    private void Awake()
    {
        // Create a COPY of the radial fill material to make this timer act independently
        _fillRenderer.material = Instantiate(_radialFillMaterial);
    }

    public void TickForSeconds(int seconds)
    {
        StartCoroutine(TickForSecondsCoroutine(seconds));
    }

    private IEnumerator TickForSecondsCoroutine(int seconds)
    {
        float currTime = 0;
        while (currTime < seconds)
        {
            currTime += Time.deltaTime;
            _fillRenderer.material.SetFloat("_Arc1", 360 * (currTime / seconds));
            yield return null;
        }
    }

}
