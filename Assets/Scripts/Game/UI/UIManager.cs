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
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private TextMeshProUGUI _timerDayText;

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

    private void Start()
    {
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
                TransitionManager.Instance.TransitionAndCall(() =>
                {
                    SceneManager.LoadScene("Night");
                });
            } else
            {
                // Failed quota. Lost.
                TransitionManager.Instance.TransitionAndCall(() =>
                {
                    SceneManager.LoadScene("Title");
                });
            }
        }
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
        _quotaText.text = "$" + earnedMoney.ToString() + "/" + GameManager.CurrentQuota;
        if (earnedMoney > GameManager.CurrentQuota)
        {
            _quotaText.color = Color.green;
        } else
        {
            _quotaText.color = Color.red;
        }
    }

}
