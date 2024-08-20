using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    CUSTOMER_ENTER = 0,
    CUSTOMER_EXIT = 1,
    CUSTOMER_PAY = 2,
    CUSTOMER_SIT_DOWN = 3,
    BUTTON_CLICK = 4,
    BUTTON_HOVER = 5,
    COOKING_FOOD = 6,
    BUILD_OBJECT = 7,
    SELL_OBJECT = 8,
    PLAYER_FOOTSTEP = 9,
    PLAYER_INTERACT = 10,
    TITLE_TO_DAY = 11,
    START_OF_NEXT_DAY = 12,
    END_OF_DAY = 13,
    TIMES_UP_ALARM = 14
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    [Header("Audio Assignments")]
    [SerializeField] private AudioClip _customerEnter;
    [SerializeField] private AudioClip _customerExit;
    [SerializeField] private AudioClip _customerPay;
    [SerializeField] private AudioClip _customerSitDown;
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _buttonHover;
    [SerializeField] private AudioClip _cookingFood;
    [SerializeField] private AudioClip _buildObject;
    [SerializeField] private AudioClip _sellObject;
    [SerializeField] private AudioClip _playerFootstep;
    [SerializeField] private AudioClip _playerInteract;
    [SerializeField] private AudioClip _titleToDay;
    [SerializeField] private AudioClip _startOfDay;
    [SerializeField] private AudioClip _endOfDay;
    [SerializeField] private AudioClip _timesUpAlarm;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume(float volume) => _audioSource.volume = volume;

    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        _audioSource.PlayOneShot(clip, _audioSource.volume * volume);
    }

    public void PlayOneShot(SFX sfx, float volume = 1)
    {
        _audioSource.PlayOneShot(GetAudioClip(sfx), _audioSource.volume * volume);
    }

    public AudioClip GetAudioClip(SFX sfx)
    {
        switch (sfx)
        {
            case SFX.CUSTOMER_ENTER:
                return _customerEnter;
            case SFX.CUSTOMER_EXIT:
                return _customerExit;
            case SFX.CUSTOMER_PAY:
                return _customerPay;
            case SFX.CUSTOMER_SIT_DOWN:
                return _customerSitDown;
            case SFX.BUTTON_CLICK:
                return _buttonClick;
            case SFX.BUTTON_HOVER:
                return _buttonHover;
            case SFX.COOKING_FOOD:
                return _cookingFood;
            case SFX.BUILD_OBJECT:
                return _buildObject;
            case SFX.SELL_OBJECT:
                return _sellObject;
            case SFX.PLAYER_FOOTSTEP:
                return _playerFootstep;
            case SFX.PLAYER_INTERACT:
                return _playerInteract;
            case SFX.TITLE_TO_DAY:
                return _titleToDay;
            case SFX.START_OF_NEXT_DAY:
                return _startOfDay;
            case SFX.END_OF_DAY:
                return _endOfDay;
            case SFX.TIMES_UP_ALARM:
                return _timesUpAlarm;
            default:
                Debug.LogWarning("SFX not found: " + sfx);
                return null;
        }
    }

}
