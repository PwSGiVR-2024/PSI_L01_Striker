using UnityEngine;
using Unity.Cinemachine;

public class MainMenuHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject overlay; // Overlay UI element

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera mainMenuCamera; // Main menu camera
    [SerializeField] private CinemachineCamera playerCamera; // Player camera

    [Header("Environment")]
    [SerializeField] private Light environmentLight; // Environment light in the scene

    private FirstPersonController playerController; // Reference to the player controller

    private void Awake()
    {
        // Find the player controller dynamically
        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController == null)
        {
            Debug.LogError("FirstPersonController not found in the scene.");
        }

        // Disable the overlay at the start
        if (overlay != null)
        {
            overlay.SetActive(false);
        }

        // Freeze player controls and show the mouse cursor
        if (playerController != null)
        {
            playerController.CanMove = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Ensure the main menu camera is active
        if (mainMenuCamera != null)
        {
            mainMenuCamera.Priority = 10; // Higher priority for main menu camera
        }
        if (playerCamera != null)
        {
            playerCamera.Priority = 0; // Lower priority for player camera
        }
    }

    public void OnPlayButtonPressed()
    {
        // Switch camera priority
        if (mainMenuCamera != null)
        {
            mainMenuCamera.Priority = 0; // Lower priority for main menu camera
        }
        if (playerCamera != null)
        {
            playerCamera.Priority = 10; // Higher priority for player camera
        }

        // Disable the environment light
        if (environmentLight != null)
        {
            environmentLight.enabled = false;
        }

        // Enable player controls and lock the cursor
        if (playerController != null)
        {
            playerController.CanMove = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the overlay
        if (overlay != null)
        {
            overlay.SetActive(false);
        }
    }

    public void OnQuitButtonPressed()
    {
        // Quit the application
        Application.Quit();
    }
}