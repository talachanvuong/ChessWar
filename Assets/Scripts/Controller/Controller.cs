using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; private set; }

    public ValueTransfer valueTransfer { get; private set; }
    public CameraController cameraController { get; private set; }
    public UIController uiController { get; private set; }
    public SceneController sceneController { get; private set; }
    public SoundController soundController { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        valueTransfer = GetComponent<ValueTransfer>();
        cameraController = GetComponent<CameraController>();
        uiController = GetComponent<UIController>();
        sceneController = GetComponent<SceneController>();
        soundController = GetComponent<SoundController>();
    }
}