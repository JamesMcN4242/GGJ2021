using PersonalFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuState : FlowStateBase
{
    private UIMainMenu m_uiMainMenu;
    private float k_maxFadeTime = 1;
    private float m_remainingTime = 0;
    private Image m_curtain;
    
    protected override bool AquireUIFromScene()
    {
        m_uiMainMenu = GameObject.Find("UIMainMenu").GetComponent<UIMainMenu>();
        m_ui = m_uiMainMenu;
        Debug.Assert(m_ui != null, "Could no find Main Menu screen.");
        return true;
    }

    protected override void HandleMessage(object message)
    {
        if (message is string msg)
        {
            switch (msg)
            {
                case "START":
                    m_remainingTime = k_maxFadeTime;
                    EndActiveState();
                    break;
                case "QUIT":
                    Debug.Log("Application Quit!");
                    Application.Quit();
                    break;
            }
        }
    }

    protected override void UpdateDismissingState()
    {
        m_remainingTime -= Time.deltaTime;
        m_uiMainMenu.SetCurtainAlpha(Mathf.Clamp01(m_remainingTime / k_maxFadeTime));
        if (m_remainingTime < 0)
        {
            EndDismissingState();
            SceneManager.LoadScene("Game");
        }
    }
}
