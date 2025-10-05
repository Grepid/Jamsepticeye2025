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

    public void AddScene(string sceneName)
    {
        // SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        StartCoroutine(LoadYourAsyncScene(sceneName));
        FindFirstObjectByType<InteractionSystem>().StopInteract();
        FindFirstObjectByType<InteractionSystem>().enabled = false;
        Records.freeze = true;
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Loaded Scene");
    }

    public void RemoveScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        FindFirstObjectByType<InteractionSystem>().enabled = true;
        Records.freeze = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
