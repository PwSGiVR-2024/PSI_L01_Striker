using UnityEngine;
using Unity.Cinemachine;

public class SettingsHandler : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera mainMenuCamera; 
    [SerializeField] private CinemachineCamera settingsCamera; 


    public void OnBackButtonPressed()
    {
        if (mainMenuCamera != null)
        {
            mainMenuCamera.Priority = 10;
        }
        if (settingsCamera != null)
        {
            settingsCamera.Priority = 0;
        }
    }
}
