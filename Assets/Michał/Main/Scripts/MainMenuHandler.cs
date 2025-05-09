using UnityEngine;
using Unity.Cinemachine;

public class MainMenuHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject overlay; // Overlay UI element (e.g., for player controls)
    [SerializeField] private GameObject canvas; // Main menu canvas

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

        // Ensure the canvas is active at the start
        if (canvas != null)
        {
            canvas.SetActive(true);
        }

        // Freeze player controls and show the mouse cursor
        if (playerController != null)
        {
            playerController.CanMove = false;
        }

        // Make the cursor visible and unlocked
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

        // Enable player controls
        if (playerController != null)
        {
            playerController.CanMove = true;
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Show the overlay
        if (overlay != null)
        {
            overlay.SetActive(true);
        }

        // Hide the canvas
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    public void OnQuitButtonPressed()
    {
        // Quit the application
        Application.Quit();
    }
}
