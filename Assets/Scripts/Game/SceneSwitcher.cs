using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    private static SceneSwitcher _instance;
    public static SceneSwitcher Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneSwitcher>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
    }
    
    /// <summary>
    /// Given a name for a scene, switches to that scene.
    /// </summary>
    public void SwitchSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
