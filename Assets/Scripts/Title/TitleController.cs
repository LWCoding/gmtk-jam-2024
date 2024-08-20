using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{

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
    }

}
