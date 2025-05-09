using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Button difficultyButton;
    public Button graphicsButton;
    public Button audioButton;
    public Button keybindsButton;

    private string currentScene = "Settings";

    void Start()
    {
        difficultyButton.onClick.AddListener(() => LoadSceneAdditive("Difficulty"));
        graphicsButton.onClick.AddListener(() => LoadSceneAdditive("Graphics"));
        audioButton.onClick.AddListener(() => LoadSceneAdditive("Audio"));
        keybindsButton.onClick.AddListener(() => LoadSceneAdditive("Keybinds"));
    }

    void LoadSceneAdditive(string sceneName)
    {
        if (currentScene != "")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        currentScene = sceneName;
    }
}
