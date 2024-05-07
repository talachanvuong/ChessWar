using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIChooseFaction : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI text;
    private ButtonAdvanced buttonAdvanced;

    private void Start()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        buttonAdvanced = GetComponent<ButtonAdvanced>();
        buttonAdvanced.clickMethod.AddListener(() => ChooseFaction());
        PerformColor();
    }

    private void OnDestroy()
    {
        buttonAdvanced.clickMethod.RemoveAllListeners();
    }

    private void PerformColor()
    {
        ColorUtility.TryParseHtmlString("#FAF3F0", out Color whiteColor);
        ColorUtility.TryParseHtmlString("#035357", out Color blackColor);

        if (Controller.Instance.valueTransfer.isWhiteMain)
        {
            background.color = whiteColor;
            text.color = blackColor;
            text.SetText("WHITE");
        }
        else
        {
            background.color = blackColor;
            text.color = whiteColor;
            text.SetText("BLACK");
        }
    }

    private void ChooseFaction()
    {
        Controller.Instance.valueTransfer.isWhiteMain = !Controller.Instance.valueTransfer.isWhiteMain;
        PerformColor();
    }
}