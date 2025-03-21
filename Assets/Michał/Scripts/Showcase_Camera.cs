using UnityEngine;
using TMPro;

public class Showcase_Camera : MonoBehaviour
{
    public float baseSpeed = 10.0f;
    public float speedMultiplier = 1.0f;
    public float minSpeedMultiplier = 0.01f;
    public float maxSpeedMultiplier = 2.0f;
    public float scrollSensitivity = 0.1f;
    public TMP_Text speedMultiplierText;

    void Start()
    {
        UpdateSpeedMultiplierText();
    }

    void Update()
    {
        HandleMovement();
        HandleSpeedChange();
    }

    void HandleMovement()
    {
        float currentSpeedMultiplier = speedMultiplier;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeedMultiplier *= 2.0f;
        }

        float speed = baseSpeed * currentSpeedMultiplier;
        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction -= transform.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += transform.up;
        }

        transform.position += direction * speed * Time.deltaTime;

        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, rotationX, Space.World);
        transform.Rotate(Vector3.left, rotationY, Space.Self);
    }

    void HandleSpeedChange()
    {
        float currentScrollSensitivity = scrollSensitivity;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentScrollSensitivity *= 2.0f;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            speedMultiplier += scroll * currentScrollSensitivity;
            speedMultiplier = Mathf.Clamp(speedMultiplier, minSpeedMultiplier, maxSpeedMultiplier);
            UpdateSpeedMultiplierText();
        }
    }

    void UpdateSpeedMultiplierText()
    {
        if (speedMultiplierText != null)
        {
            speedMultiplierText.text = $"Speed Multiplier: {speedMultiplier:F2}";
        }
    }
}


