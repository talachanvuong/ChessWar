using UnityEngine;

public class UIStartGame : MonoBehaviour
{
    private ButtonAdvanced buttonAdvanced;

    private void Start()
    {
        buttonAdvanced = GetComponent<ButtonAdvanced>();
        buttonAdvanced.clickMethod.AddListener(() => StartGame());
    }

    private void OnDestroy()
    {
        buttonAdvanced.clickMethod.RemoveAllListeners();
    }

    private void StartGame()
    {
        Controller.Instance.sceneController.LoadScene(1);
    }
}