using UnityEngine;
using System;
using TMPro;

public class UIPerformBotThinking : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        Controller.Instance.uiController.OnBotThinking += UIPerformBotThinking_OnBotThinking;
        Controller.Instance.uiController.OnBotDoneThinking += UIPerformBotThinking_OnBotDoneThinking;
    }

    private void OnDestroy()
    {
        Controller.Instance.uiController.OnBotThinking -= UIPerformBotThinking_OnBotThinking;
        Controller.Instance.uiController.OnBotDoneThinking -= UIPerformBotThinking_OnBotDoneThinking;
    }

    private void UIPerformBotThinking_OnBotThinking(object sender, EventArgs e)
    {
        text.enabled = true;
    }

    private void UIPerformBotThinking_OnBotDoneThinking(object sender, EventArgs e)
    {
        text.enabled = false;
    }
}