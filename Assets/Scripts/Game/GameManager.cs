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
                Initialize();
            }
            return _instance;
        }
    }

    public static int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged?.Invoke(value);
        }
    }
    private static int _money;

    public static int Timer
    {
        get => _timer;
        set
        {
            _timer = value;
            OnTimerChanged?.Invoke(value, SECS_IN_DAY);
        }
    }
    private static int _timer;
    private const int SECS_IN_DAY = 60;

    public const int MONEY_PER_OMELETTE = 8;

    public static Dictionary<Vector3Int, TileObject> RestaurantTiles = new();  // To persist between scenes

    public static int TilemapLeftLimit = -2;
    public static int TilemapRightLimit = 2;
    public static int TilemapDownLimit = -2;
    public static int TilemapUpLimit = 2;

    public static Action<int> OnMoneyChanged = null;  // Param is new amount of money
    public static Action<int, int> OnTimerChanged = null;  // Param is current time and max time

    public static List<TileObject> AllTileObjects = new();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        InvokeRepeating(nameof(IncrementTimer), 0, 1);
    }

    public static void Initialize()
    {
        AllTileObjects = new(Resources.LoadAll<TileObject>("Special Tiles"));
        Money = 1000;  // Starting amount of money
        Timer = 0;  // Start off at zero seconds

        if (RestaurantTiles.Count == 0)
        {
            RestaurantTiles[new(-2, -2)] = AllTileObjects[0];  // Stoves
            RestaurantTiles[new(-2, -1)] = AllTileObjects[0];
            RestaurantTiles[new(1, 1)] = AllTileObjects[1];  // Table
        }
    }

    /// <summary>
    /// Increment the timer if we haven't hit the maximum seconds in a day.
    /// </summary>
    private void IncrementTimer()
    {
        if (Timer == SECS_IN_DAY) { return; }
        Timer++;
    }

}
