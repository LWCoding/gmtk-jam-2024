using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Image _timerFillImage;

    private void Awake()
    {
        GameManager.Instance.OnMoneyChanged += UpdateMoneyText;
        GameManager.Instance.OnTimerChanged += UpdateTimerValue;
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
    }

}
