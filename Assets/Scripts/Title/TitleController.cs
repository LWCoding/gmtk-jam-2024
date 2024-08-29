using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private Animator _textAnimator;

    // Reset all GameManager values before game start
    void Start()
    {
        GameManager.DayNumber = 0;
        GameManager.TilemapLeftLimit = -2;
        GameManager.TilemapRightLimit = 2;
        GameManager.TilemapDownLimit = -2;
        GameManager.TilemapUpLimit = 2;
        GameManager.CameraZoom = 3.4f;
        GameManager.CameraYOffset = 0;
        GameManager.Money = 100;
        GameManager.InitializedKitchen = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlaySoundAndStart());
        }
    }

    private IEnumerator PlaySoundAndStart()
    {
        AudioManager.Instance.PlayOneShot(SFX.TITLE_TO_DAY);
        _textAnimator.Play("Blink");
        yield return new WaitForSeconds(0.6f);
        TransitionManager.Instance.TransitionAndCall(() =>
        {
            SceneManager.LoadScene("Day");
        });
    }

}
