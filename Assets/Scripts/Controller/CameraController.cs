using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += CameraController_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CameraController_sceneLoaded;
    }

    private void CameraController_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Camera.main.orthographicSize = 8.04f * Screen.height / Screen.width * 0.5f;
    }
}