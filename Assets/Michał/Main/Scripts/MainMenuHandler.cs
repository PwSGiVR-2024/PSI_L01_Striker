using UnityEngine;
using Unity.Cinemachine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainMenuCamera;
    [SerializeField] private CinemachineCamera settingsCamera;
    [SerializeField] private CinemachineCamera achievmentCamera; // Not currently used
    [SerializeField] private CinemachineCamera playerCamera;

    [SerializeField] private Light environmentLight; // Turned off when entering the game

    [SerializeField] private GameObject mainMenuCanvas; 
    [SerializeField] private GameObject settingsCanvas; 
    [SerializeField] private GameObject overlayCanvas; 

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

        // Disable gameplay canvas on start
        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(false);
        }
    }

    public void OnPlayButtonPressed()
    {
        mainMenuCamera.Priority = 0;
        playerCamera.Priority = 10;

        if (environmentLight != null)
        {
            environmentLight.enabled = false; // Disable light for gameplay
        }

        if (playerController != null)
        {
            playerController.CanMove = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Deactivate menu and settings canvases
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }

        // Activate gameplay canvas
        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(true);
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

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
