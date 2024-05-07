using UnityEngine;

public class UIErrorConnect : MonoBehaviour
{
    [SerializeField] private CanvasGroup errorConnect;

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Controller.Instance.uiController.DisableAccess();
            errorConnect.alpha = 1;
        }
    }
}