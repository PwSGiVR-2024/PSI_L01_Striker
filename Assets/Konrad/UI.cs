using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        UpdateHealth(100);
    }

    private void OnEnable()
    {
        FirstPersonController.OnDamage += UpdateHealth;
        FirstPersonController.OnHeal += UpdateHealth;
    }

    private void OnDisable()
    {
        FirstPersonController.OnDamage -= UpdateHealth;
        FirstPersonController.OnHeal -= UpdateHealth;
    }

    private void UpdateHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
