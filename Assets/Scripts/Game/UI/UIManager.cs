using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _quotaText;
    [SerializeField] private TextMeshProUGUI _quotaMaxText;
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private TextMeshProUGUI _timerDayText;
    [Header("End Screen Assignments")]
    [SerializeField] private Animator _dayEndAnimator;
    [SerializeField] private TextMeshProUGUI _endText;
    [SerializeField] private TextMeshProUGUI _endSubtext;

    private int _moneyAtStartOfDay;

    private void OnEnable()
    {
        _moneyAtStartOfDay = GameManager.Money;
        if (_moneyText != null)
        {
            GameManager.OnMoneyChanged += UpdateMoneyText;
        }
        if (_timerFillImage != null)
        {
            GameManager.OnTimerChanged += UpdateTimerValue;
        }
        if (_quotaText != null)
        {
            GameManager.OnMoneyChanged += UpdateQuotaText;
        }
    }

    private void OnDisable()
    {
        GameManager.OnMoneyChanged -= UpdateMoneyText;
        GameManager.OnTimerChanged -= UpdateTimerValue;
        GameManager.OnMoneyChanged -= UpdateQuotaText;
    }

    private void Awake()
    {
        if (_timerFillImage != null)
        {
            UpdateTimerValue(GameManager.Timer, GameManager.SECS_IN_DAY);
        }
        if (_moneyText != null)
        {
            UpdateMoneyText(GameManager.Money);  // Update money amount to the global one
        }
        if (_quotaText != null)
        {
            UpdateQuotaText(GameManager.Money);
        }
    }

    /// <summary>
    /// Given an amount of money, updates the UI text to show that number.
    /// </summary>
    private void UpdateMoneyText(int currMoney)
    {
        _moneyText.text = "$" + currMoney.ToString();
    }

    /// <summary>
    /// Given an amount of time that has passed and the total amount of time,
    /// updates the UI to show that time.
    /// </summary>
    private void UpdateTimerValue(int timeElapsed, int totalTime)
    {
        _timerFillImage.fillAmount = (float)timeElapsed / totalTime;
        _timerDayText.text = CalculateTimeString(timeElapsed, totalTime);
        if (timeElapsed == totalTime)
        {
            if (GameManager.Money - _moneyAtStartOfDay >= GameManager.CurrentQuota)
            {
                // Met quota!
                AudioManager.Instance.PlayOneShot(SFX.DAY_WIN);
                StartCoroutine(EndScreenCoroutine("Quota Met!", "Time to prepare for tomorrow...", Color.green, "Night"));
            } else
            {
                // Failed quota. Lost.
                AudioManager.Instance.PlayOneShot(SFX.DAY_LOSE);
                StartCoroutine(EndScreenCoroutine("Quota Failed!", "You're fired...", Color.red, "Title"));
            }
        }
    }

    private IEnumerator EndScreenCoroutine(string mainText, string subText, Color mainTextColor, string sceneToTransitionTo)
    {
        // Stop stuff from happening
        FindObjectOfType<CustomerSpawner>().enabled = false;
        FindObjectOfType<PlayerMovement>().enabled = false;
        _endText.text = mainText;
        _endText.color = mainTextColor;
        _endSubtext.text = subText;
        _dayEndAnimator.Play("Show");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => _dayEndAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        TransitionManager.Instance.TransitionAndCall(() =>
        {
            SceneManager.LoadScene(sceneToTransitionTo);
        });
    }

    /// <summary>
    /// Retrieve a string between 9am and 6pm depending on the amount of time
    /// that has elapsed, determined by currTime and maxTime.
    /// </summary>
    public static string CalculateTimeString(int currTime, int maxTime)
    {
        int startTimeMinutes = 9 * 60;  // 9:00am in minutes
        int endTimeMinutes = 18 * 60;   // 6:00pm in minutes
        int totalDuration = endTimeMinutes - startTimeMinutes;
        double timeFraction = (double)currTime / maxTime;
        int interpolatedTimeMinutes = startTimeMinutes + (int)(timeFraction * totalDuration);
        int hours = interpolatedTimeMinutes / 60;

        // Determine AM/PM
        string period = hours < 12 ? "\nam" : "\npm";

        // Adjust hour to 12-hour format
        if (hours > 12)
        {
            hours -= 12;
        }
        else if (hours == 0)
        {
            hours = 12;
        }

        // Format the time string to show only hours
        string timeString = $"{hours} {period}";

        return timeString;
    }

    /// <summary>
    /// Updates the quota text.
    /// </summary>
    private void UpdateQuotaText(int money)
    {
        int earnedMoney = money - _moneyAtStartOfDay;
        _quotaText.text = "$" + earnedMoney.ToString();
        _quotaMaxText.text = "$" + GameManager.CurrentQuota;
        if (earnedMoney > GameManager.CurrentQuota)
        {
            _quotaText.color = new Color(0, 0.55f, 0);
        } else
        {
            _quotaText.color = Color.red;
        }
    }

}
