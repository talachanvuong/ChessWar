using UnityEngine;
using TMPro;
using System;

public class UIPerformTurnCount : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        Controller.Instance.uiController.OnTurnCountChange += UIPerformTurnCount_OnTurnCountChange;
    }

    private void OnDestroy()
    {
        Controller.Instance.uiController.OnTurnCountChange -= UIPerformTurnCount_OnTurnCountChange;
    }

    private void UIPerformTurnCount_OnTurnCountChange(object sender, EventArgs e)
    {
        text.SetText(GameController.Instance.TurnCount.ToString());
    }
}