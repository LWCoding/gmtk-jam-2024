using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged?.Invoke(value);
        }
    }
    private int _money;

    public int Timer
    {
        get => _timer;
        set
        {
            _timer = value;
            OnTimerChanged?.Invoke(value, SECS_IN_DAY);
        }
    }
    private int _timer;
    private const int SECS_IN_DAY = 180;

    public int TilemapLeftLimit = -2;
    public int TilemapRightLimit = 2;
    public int TilemapDownLimit = -2;
    public int TilemapUpLimit = 2;

    public Action<int> OnMoneyChanged = null;  // Param is new amount of money
    public Action<int, int> OnTimerChanged = null;  // Param is current time and max time

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        Money = 1000;  // Starting amount of money
        Timer = 0;  // Start off at zero seconds
        InvokeRepeating(nameof(IncrementTimer), 0, 1);
    }

    private void IncrementTimer()
    {
        Timer++;
    }

}
