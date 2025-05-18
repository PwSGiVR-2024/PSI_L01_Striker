using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

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
    [SerializeField] private CanvasGroup overlayCanvasGroup;

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
        //achievmentCamera.Priority = 0;
        
        if (overlayCanvasGroup != null)
        {
            overlayCanvasGroup.alpha = 0;
            overlayCanvasGroup.interactable = false;
            overlayCanvasGroup.blocksRaycasts = false;
        }
        
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
            environmentLight.enabled = false;
        }

        if (playerController != null)
        {
            playerController.CanMove = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(true);
        }

        StartCoroutine(HandlePlayTransition());
    }

    private IEnumerator HandlePlayTransition()
    {
        yield return StartCoroutine(FadeInOverlay());

        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }
    }

    private IEnumerator FadeInOverlay()
    {
        float duration = 3.0f;
        float elapsedTime = 0f;

        overlayCanvasGroup.interactable = true;
        overlayCanvasGroup.blocksRaycasts = true;

        while (elapsedTime < duration)
        {
            overlayCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        overlayCanvasGroup.alpha = 1;
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
