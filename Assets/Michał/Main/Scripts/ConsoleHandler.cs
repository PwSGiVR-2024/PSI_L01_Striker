using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConsoleHandler : FirstPersonController
{
    [Header("UI Elements")]
    [SerializeField] private GameObject consoleCanvas; // Canvas for the console
    [SerializeField] private TMP_InputField inputField; // Input field for commands
    [SerializeField] private TMP_Text chatHistory; // Text for chat history
    [SerializeField] private ScrollRect scrollRect; // Scroll Rect for chat history
    [SerializeField] private GameObject mainMenuCanvas; // Reference to the Main Menu Canvas

    [Header("Audio")]
    [SerializeField] private AudioSource ambientSound; // Ambient sound to pause/resume

    [Header("Lighting")]
    [SerializeField] private Light directionalLight; // Directional light in the scene
    [SerializeField] private Light flashlight; // Flashlight attached to the player

    private bool isGodMode = false; // Tracks if god mode is active
    private bool isNoClip = false; // Tracks if noclip mode is active

    public static bool IsConsoleActive { get; private set; } = false; // Tracks if the console is active

    private int maxLines = 50; // Maksymalna liczba linii w historii

    private void Awake()
    {
        // Ensure the console is hidden at the start
        if (consoleCanvas != null)
        {
            consoleCanvas.SetActive(false); // Ukryj konsolê na pocz¹tku
        }

        // Add default message to chat history
        if (chatHistory != null)
        {
            chatHistory.text = "Type /help to see all available commands.\n";
        }
    }

    private void Update()
    {
        // Toggle console visibility on "~" key press
        if (Input.GetKeyDown(KeyCode.BackQuote)) // "~" key
        {
            ToggleConsole();
        }

        // Handle command execution on "Enter" key press
        if (IsConsoleActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand(inputField.text);
            inputField.text = ""; // Clear the input field
            inputField.ActivateInputField(); // Reactivate the input field for further typing
        }

        // Obs³uga przewijania strza³kami
        if (IsConsoleActive && scrollRect != null)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                scrollRect.verticalNormalizedPosition += 0.01f; // Przewijanie w górê
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                scrollRect.verticalNormalizedPosition -= 0.01f; // Przewijanie w dó³
            }
        }

        // Obs³uga przewijania myszk¹
        if (IsConsoleActive && Input.mouseScrollDelta.y != 0)
        {
            scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y * 0.1f;
        }
    }

    private void ToggleConsole()
    {
        IsConsoleActive = !IsConsoleActive;

        // Show or hide the console canvas
        if (consoleCanvas != null)
        {
            consoleCanvas.SetActive(IsConsoleActive);
        }

        // Freeze or unfreeze the game
        if (IsConsoleActive)
        {
            Time.timeScale = 0; // Zamro¿enie czasu
            Debug.Log("Game paused (Time.timeScale = 0)");
        }
        else
        {
            Time.timeScale = 1; // Odmro¿enie czasu
            Debug.Log("Game resumed (Time.timeScale = 1)");
        }

        // Pause or resume ambient sound
        if (ambientSound != null)
        {
            if (IsConsoleActive)
                ambientSound.Pause();
            else
                ambientSound.UnPause();
        }

        // Handle cursor visibility and lock state
        if (IsConsoleActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // Poka¿ myszkê
        }
        else
        {
            // Check if Main Menu is active
            if (mainMenuCanvas != null && mainMenuCanvas.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; // Poka¿ myszkê, jeœli Main Menu jest aktywne
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; // Ukryj myszkê, jeœli Main Menu nie jest aktywne
            }
        }

        // Focus on the input field when the console is active
        if (IsConsoleActive && inputField != null)
        {
            inputField.ActivateInputField();
        }
    }

    private void ExecuteCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return; // Ignore empty commands
        }

        string[] args = command.ToLower().Split(' '); // Split command into parts
        string baseCommand = args[0];

        switch (baseCommand)
        {
            case "/help":
                AddToChatHistory("Available commands:\n" +
                                 "/help - Show this help message\n" +
                                 "/quit - Quit the game\n" +
                                 "/tp x y z - Teleport to coordinates\n" +
                                 $"/sethealth value - Set player health (Default: {maxHealth})\n" +
                                 $"/setstamina value - Set player stamina (Default: {maxStamina})\n" +
                                 $"/setgravity value - Set gravity (Default: {gravity})\n" +
                                 "/timescale value - Set game speed\n" +
                                 "/god - Toggle god mode\n" +
                                 "/noclip - Toggle noclip mode\n" +
                                 "/light intensity value - Set directional light intensity\n" +
                                 "/light color r g b - Set directional light color\n" +
                                 "/flashlight on/off - Toggle flashlight\n" +
                                 "/flashlight intensity value - Set flashlight intensity");
                break;

            case "/quit":
                AddToChatHistory("Quitting the game...");
                Application.Quit();
                break;

            // Other commands...

            default:
                AddToChatHistory($"Invalid command: {command}. Type /help to see all available commands.");
                break;
        }
    }

    private void AddToChatHistory(string message)
    {
        if (chatHistory != null)
        {
            chatHistory.text += message + "\n";

            // Usuñ najstarsze linie, jeœli przekroczono limit
            string[] lines = chatHistory.text.Split('\n');
            if (lines.Length > maxLines)
            {
                chatHistory.text = string.Join("\n", lines, lines.Length - maxLines, maxLines);
            }

            // Automatyczne przewijanie do najnowszej wiadomoœci
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
