using UnityEngine;
using Unity.Cinemachine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainMenuCamera;
    [SerializeField] private CinemachineCamera settingsCamera;
    [SerializeField] private CinemachineCamera achievmentCamera;
    [SerializeField] private CinemachineCamera playerCamera;

    [SerializeField] private Light environmentLight;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject overlayCanvas; // Gameplay UI

    private FirstPersonController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.CanMove = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mainMenuCamera.Priority = 10;
        settingsCamera.Priority = 0;
        achievmentCamera.Priority = 0;

        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(false); // Ensure overlay is hidden on start
        }
    }

    public void OnPlayButtonPressed()
    {
        mainMenuCamera.Priority = 0;
        playerCamera.Priority = 10;

        if (environmentLight != null)
        {
            environmentLight.enabled = false; // Turn off menu lighting
        }

        if (playerController != null)
        {
            playerController.CanMove = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }
        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(true); // Show overlay on play
        }
    }

    public void OnSettingsButtonPressed()
    {
        mainMenuCamera.Priority = 0;
        settingsCamera.Priority = 10;
    }

    public void OnAchievementsButtonPressed()
    {
        mainMenuCamera.Priority = 0;
        achievmentCamera.Priority = 10;
    }

    public void OnBackToMainMenuPressed()
    {
        settingsCamera.Priority = 0;
        achievmentCamera.Priority = 0;
        mainMenuCamera.Priority = 10;
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
