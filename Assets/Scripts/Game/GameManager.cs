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

    public static Dictionary<Vector3Int, TileObject> RestaurantTiles = new();  // To persist between scenes

    public static int TilemapLeftLimit = -2;
    public static int TilemapRightLimit = 2;
    public static int TilemapDownLimit = -2;
    public static int TilemapUpLimit = 2;
    public static float CameraZoom = 3.4f;
    public static float CameraYOffset = 0;

    public static int DayNumber = 0;  // Running count for current day
    public static int CurrentQuota  // Quota needed to complete day
    {
        get
        {
            int quota = 5 * MONEY_PER_OMELETTE;
            for (int i = 0; i < DayNumber; i++)
            {
                quota += (int)Mathf.Ceil(DayNumber / 2) * MONEY_PER_OMELETTE;
            }
            return quota;
        }
    }

    public static int MONEY_PER_OMELETTE = 8;

    public int DecorBuff = 0;  // Decor buff for current level

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
        AllTileObjects.Sort((a, b) => a.CostToBuy - b.CostToBuy);
        Money = 100;  // Starting amount of money
        Timer = 0;  // Start off at zero seconds

        if (RestaurantTiles.Count == 0)
        {
            RestaurantTiles[new(-1, -1)] = AllTileObjects[4];  // Stoves
            RestaurantTiles[new(0, -1)] = AllTileObjects[4];
            RestaurantTiles[new(1, 1)] = AllTileObjects[2];  // Table
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayOneShot(SFX.START_OF_NEXT_DAY);
        Camera.main.orthographicSize = CameraZoom;
        Camera.main.transform.position += new Vector3(0, CameraYOffset, -10);
        StartCoroutine(WaitThenScan());
    }

    private IEnumerator WaitThenScan()
    {
        yield return new WaitForEndOfFrame();
        if (AstarPath.active != null)
        {
            AstarPath.active.Scan();
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
