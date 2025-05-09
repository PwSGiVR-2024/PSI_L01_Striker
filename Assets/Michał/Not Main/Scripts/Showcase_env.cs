using UnityEngine;

public class Showcase_Env : MonoBehaviour
{
    private bool isNight = false;
    private Light directionalLight;

    private Color originalAmbientLight;
    private float originalIntensityMultiplier;
    private Color originalShadowColor;
    private float originalReflectionIntensity;

    void Start()
    {
        directionalLight = GameObject.FindGameObjectWithTag("DLight").GetComponent<Light>();

        originalAmbientLight = RenderSettings.ambientLight;
        originalIntensityMultiplier = RenderSettings.ambientIntensity;
        originalShadowColor = RenderSettings.subtractiveShadowColor;
        originalReflectionIntensity = RenderSettings.reflectionIntensity;
    }

    void Update()
    {
        HandleDayNightToggle();
    }

    void HandleDayNightToggle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isNight = !isNight;
            if (isNight)
            {
                if (directionalLight != null)
                {
                    directionalLight.enabled = false;
                }
                RenderSettings.ambientLight = new Color(5f / 255f, 5f / 255f, 5f / 255f);
                RenderSettings.ambientIntensity = 0.1f;
                RenderSettings.subtractiveShadowColor = Color.black;
                RenderSettings.reflectionIntensity = 0.0f;
            }
            else
            {
                if (directionalLight != null)
                {
                    directionalLight.enabled = true;
                }
                RenderSettings.ambientLight = originalAmbientLight;
                RenderSettings.ambientIntensity = originalIntensityMultiplier;
                RenderSettings.subtractiveShadowColor = originalShadowColor;
                RenderSettings.reflectionIntensity = originalReflectionIntensity;
            }
            DynamicGI.UpdateEnvironment();
        }
    }
}

