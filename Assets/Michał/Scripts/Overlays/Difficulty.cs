using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;
    public Button hellButton;
    public Button customButton;
    public Button backButton;

    void Start()
    {
        easyButton.onClick.AddListener(SetEasyDifficulty);
        normalButton.onClick.AddListener(SetNormalDifficulty);
        hellButton.onClick.AddListener(SetHellDifficulty);
        customButton.onClick.AddListener(LoadCustomDifficulty);
        backButton.onClick.AddListener(BackToSettings);
    }

    void SetEasyDifficulty()
    {
        // Ustawienia dla poziomu trudno�ci Easy
    }

    void SetNormalDifficulty()
    {
        // Ustawienia dla poziomu trudno�ci Normal
    }

    void SetHellDifficulty()
    {
        // Ustawienia dla poziomu trudno�ci Hell
    }

    void LoadCustomDifficulty()
    {
        SceneManager.UnloadSceneAsync("Difficulty");
        SceneManager.LoadScene("CustomDif", LoadSceneMode.Additive);
    }

    void BackToSettings()
    {
        SceneManager.UnloadSceneAsync("Difficulty");
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }
}

