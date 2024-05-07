using UnityEngine;

public class UIBackToMenu : MonoBehaviour
{
    private ButtonAdvanced buttonAdvanced;

    private void Start()
    {
        buttonAdvanced = GetComponent<ButtonAdvanced>();
        buttonAdvanced.clickMethod.AddListener(() => BackToMenu());
    }

    private void OnDestroy()
    {
        buttonAdvanced.clickMethod.RemoveAllListeners();
    }

    private void BackToMenu()
    {
        Controller.Instance.sceneController.LoadScene(0);
    }
}