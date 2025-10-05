using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance { get;  private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
