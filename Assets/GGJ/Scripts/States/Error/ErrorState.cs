﻿using PersonalFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorState : FlowStateBase
{
    private string m_message;
    private UIError m_uiError;
    private bool m_showReconnect;
    
    public ErrorState(string message, bool showReconnect = true)
    {
        m_message = message;
        m_showReconnect = showReconnect;
    }

    protected override bool AquireUIFromScene()
    {
        m_uiError = GameObject.Find("UIError").GetComponent<UIError>();
        m_ui = m_uiError;
        Debug.Assert(m_uiError != null, "Failed to find UI Error screen.");
        return true;
    }

    protected override void StartPresentingState()
    {
        m_uiError.ShowReconnect(m_showReconnect);
        m_uiError.SetMessage(m_message);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    protected override void HandleMessage(object message)
    {
        if (message is string msg)
        {
            switch (msg)
            {
                case "RECONNECT":
                    ControllingStateStack.ChangeState(new ConnectingState());
                    break;
                case "TO_MENU":
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
    }
}
