using UnityEngine;
using UnityEngine.UI;

public class StaminaOverlay : MonoBehaviour
{
    [Header("Overlay Options")]
    [SerializeField] private FirstPersonController player;
    [SerializeField] private Image staminaOverlayImage;
    [SerializeField] private float maxAlpha = 1.0f;

    private void OnEnable()
    {
        FirstPersonController.OnStaminaChange += UpdateStaminaOverlay;
    }

    private void OnDisable()
    {
        FirstPersonController.OnStaminaChange -= UpdateStaminaOverlay;
    }

    private void Start()
    {
        UpdateStaminaOverlay(player.GetCurrentStamina());
    }

    private void UpdateStaminaOverlay(float currentStamina)
    {
        float staminaPercentage = currentStamina / player.GetMaxStamina();
        float alphaValue = Mathf.Lerp(maxAlpha, 0, staminaPercentage);

        Color overlayColor = staminaOverlayImage.color;
        overlayColor.a = alphaValue;
        staminaOverlayImage.color = overlayColor;
    }
}
