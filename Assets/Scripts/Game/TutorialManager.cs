using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static bool HasPlayedTutorialBefore = false;

    [Header("Object Assignments")]
    [SerializeField] private Animator _tutorialAnimator;

    private bool _isHiding = false;

    private void Start()
    {
        if (!HasPlayedTutorialBefore)
        {
            StartCoroutine(WaitUntilFadeIsGoneCoroutine());
            _tutorialAnimator.gameObject.SetActive(true);
            HasPlayedTutorialBefore = true;
        } else
        {
            _tutorialAnimator.gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitUntilFadeIsGoneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    public void HideTutorialObject()
    {
        Time.timeScale = 1;
        if (_isHiding) { return; }
        _tutorialAnimator.Play("Hide");
        _isHiding = true;
    }

}
