using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public event EventHandler OnTurnCountChange;
    public event EventHandler OnBotThinking;
    public event EventHandler OnBotDoneThinking;
    public event EventHandler OnEndGame;

    private EventSystem eventSystem;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UIController_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UIController_sceneLoaded;
    }

    private void UIController_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        eventSystem = GameObject.FindGameObjectWithTag(ConstantAdvanced.EVENT_SYSTEM).GetComponent<EventSystem>();
    }

    public void DisableAccess()
    {
        eventSystem.enabled = false;
    }

    public void EnableAccess()
    {
        eventSystem.enabled = true;
    }

    public void ExecuteOnTurnCountChange()
    {
        OnTurnCountChange?.Invoke(this, EventArgs.Empty);
    }

    public void ExecuteOnBotThinking()
    {
        OnBotThinking?.Invoke(this, EventArgs.Empty);
    }

    public void ExecuteOnBotDoneThinking()
    {
        OnBotDoneThinking?.Invoke(this, EventArgs.Empty);
    }

    public void ExecuteOnEndGame()
    {
        OnEndGame?.Invoke(this, EventArgs.Empty);
    }
}