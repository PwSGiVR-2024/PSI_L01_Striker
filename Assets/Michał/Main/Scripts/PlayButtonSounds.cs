using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayButtonSounds : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Audio source for playing sounds
    [SerializeField] private AudioClip defaultHoverSound; // Default sound for hovering over buttons
    [SerializeField] private AudioClip defaultClickSound; // Default sound for clicking buttons

    [Header("Button-Specific Sounds")]
    [SerializeField] private List<ButtonSoundConfig> buttonSoundConfigs; // List of button-specific sound configurations

    private Dictionary<Button, ButtonSoundConfig> buttonSoundMap; // Map for quick lookup of button sounds

    private void Awake()
    {
        // Initialize the button sound map
        buttonSoundMap = new Dictionary<Button, ButtonSoundConfig>();

        // Assign sounds to buttons
        foreach (var config in buttonSoundConfigs)
        {
            if (config.button != null)
            {
                buttonSoundMap[config.button] = config;

                // Add event listeners to the button
                AddButtonListeners(config.button);
            }
        }
    }

    private void AddButtonListeners(Button button)
    {
        // Add hover sound
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Add OnPointerEnter event for hover sound
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        hoverEntry.callback.AddListener((data) => PlayHoverSound(button));
        trigger.triggers.Add(hoverEntry);

        // Add OnClick event for click sound
        button.onClick.AddListener(() => PlayClickSound(button));
    }

    private void PlayHoverSound(Button button)
    {
        if (buttonSoundMap.TryGetValue(button, out ButtonSoundConfig config) && config.hoverSound != null)
        {
            audioSource.PlayOneShot(config.hoverSound);
        }
        else if (defaultHoverSound != null)
        {
            audioSource.PlayOneShot(defaultHoverSound);
        }
    }

    private void PlayClickSound(Button button)
    {
        if (buttonSoundMap.TryGetValue(button, out ButtonSoundConfig config) && config.clickSound != null)
        {
            audioSource.PlayOneShot(config.clickSound);
        }
        else if (defaultClickSound != null)
        {
            audioSource.PlayOneShot(defaultClickSound);
        }
    }

    [System.Serializable]
    public class ButtonSoundConfig
    {
        public Button button; // Reference to the button
        public AudioClip hoverSound; // Sound for hovering over the button
        public AudioClip clickSound; // Sound for clicking the button
    }
}
