using UnityEngine;

public class MainManuBackGroundEvents : MonoBehaviour
{
    public float speed = 10.0f;
    public float bobbingSpeed = 1f;
    public float bobbingAmount = 10f;
    public float rotationSpeed = 45f; // Prędkość obrotu w stopniach na sekundę
    private Vector3 startPosition = new Vector3(0, 11, -50);
    private Vector3 endPosition = new Vector3(0, 11, 50);
    private GameObject mainCamera;
    private float defaultPosY;
    private float timer = 0.0f;
    private bool movingForward = true;
    private bool isRotating = false;
    private bool uiEnabled = true;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("Player");
        if (mainCamera != null)
        {
            mainCamera.transform.position = startPosition;
            defaultPosY = mainCamera.transform.localPosition.y;
        }
    }

    void FixedUpdate()
    {
        if (mainCamera != null)
        {
            if (!isRotating)
            {
                MoveCamera();
            }
            HandleHeadbob();
        }
    }

    void MoveCamera()
    {
        if (movingForward)
        {
            mainCamera.transform.position += Vector3.forward * speed * Time.fixedDeltaTime;
            if (mainCamera.transform.position.z >= endPosition.z)
            {
                StartCoroutine(Rotate(180));
                movingForward = false;
            }
        }
        else
        {
            mainCamera.transform.position -= Vector3.forward * speed * Time.fixedDeltaTime;
            if (mainCamera.transform.position.z <= startPosition.z)
            {
                StartCoroutine(Rotate(180));
                movingForward = true;
            }
        }
    }

    void HandleHeadbob()
    {
        timer += Time.fixedDeltaTime * bobbingSpeed;
        float newY = defaultPosY + Mathf.Sin(timer) * bobbingAmount;
        mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, newY, mainCamera.transform.localPosition.z);
    }

    System.Collections.IEnumerator Rotate(float angle)
    {
        isRotating = true;
        float targetAngle = mainCamera.transform.eulerAngles.y + angle;
        while (Mathf.Abs(Mathf.DeltaAngle(mainCamera.transform.eulerAngles.y, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;
            mainCamera.transform.rotation = Quaternion.RotateTowards(mainCamera.transform.rotation, Quaternion.Euler(0, targetAngle, 0), step);
            yield return new WaitForFixedUpdate();
        }
        mainCamera.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        isRotating = false;
    }

    public void ToggleUI()
    {
        uiEnabled = !uiEnabled;
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject uiElement in uiElements)
        {
            Canvas canvas = uiElement.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = uiEnabled;
            }
        }
    }
}

