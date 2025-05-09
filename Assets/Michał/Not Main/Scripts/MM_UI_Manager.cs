using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MM_UI_Manager : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button achievementsButton;

    private string currentScene = "";

    void Start()
    {
        playButton.onClick.AddListener(() => LoadSceneAdditive("Play"));
        settingsButton.onClick.AddListener(() => LoadSceneAdditive("Settings"));
        achievementsButton.onClick.AddListener(() => LoadSceneAdditive("Achievements"));
    }

    void LoadSceneAdditive(string sceneName)
    {
        UnloadAllScenesExceptMain();

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        currentScene = sceneName;
    }

    void UnloadAllScenesExceptMain()
    {
        List<Scene> scenesToUnload = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "MainMenu") 
            {
                scenesToUnload.Add(scene);
            }
        }

        foreach (Scene scene in scenesToUnload)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
