using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{

    public static bool HasFadedInFromPrevScene = false;

    private static TransitionManager _instance;
    public static TransitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TransitionManager>();
            }
            return _instance;
        }
    }

    [Header("Object Assignments")]
    [SerializeField] private Animator _transitionAnimator;

    private void Awake()
    {
        _transitionAnimator.gameObject.SetActive(false);
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// At the start of the game, if we've faded to black, fade back into transparency.
    /// </summary>
    private void Start()
    {
        if (HasFadedInFromPrevScene)
        {
            TransitionFadeOut();
        }
    }

    /// <summary>
    /// Given code to invoke, invokes it after playing the transition animation.
    /// </summary>
    public void TransitionAndCall(Action codeToInvoke)
    {
        StartCoroutine(TransitionAndCallCoroutine(codeToInvoke));
    }

    private IEnumerator TransitionAndCallCoroutine(Action codeToInvoke)
    {
        TransitionFadeIn();
        // Wait until animation is done, then invoke code
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => _transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        codeToInvoke?.Invoke();
    }

    /// <summary>
    /// Makes the "screen filler" fade from transparent to opaque.
    /// </summary>
    private void TransitionFadeIn()
    {
        StartCoroutine(LerpAudio(1, 0));
        _transitionAnimator.gameObject.SetActive(true);
        _transitionAnimator.Play("Show");
        HasFadedInFromPrevScene = true;
    }

    /// <summary>
    /// Makes the "screen filler" fade from opaque to transparent.
    /// </summary>
    private void TransitionFadeOut()
    {
        StartCoroutine(LerpAudio(0, 1));
        _transitionAnimator.gameObject.SetActive(true);
        _transitionAnimator.Play("Hide");
        HasFadedInFromPrevScene = false;
    }

    private IEnumerator LerpAudio(float fromVolume, float toVolume)
    {
        float currTime = 0;
        float timeToWait = 0.2f;
        while (currTime < timeToWait)
        {
            currTime += Time.deltaTime;
            AudioManager.Instance.SetVolume(Mathf.Lerp(fromVolume, toVolume, currTime / timeToWait));
            yield return null;
        }
    }

}
