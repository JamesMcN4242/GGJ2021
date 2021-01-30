using PersonalFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuState : FlowStateBase
{
    private UIMainMenu m_uiMainMenu;
    private float k_maxFadeTime = 1;
    private float m_currentTime = 0;
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
        if (CurrentStatus == Status.ACTIVE)
        {
            if (message is string msg)
            {
                switch (msg)
                {
                    case "START":
                        m_currentTime = 0;
                        EndActiveState();
                        break;
                    case "QUIT":
                        Debug.Log("Application Quit!");
                        Application.Quit();
                        break;
                }
            }   
        }
    }

    protected override void UpdateDismissingState()
    {
        m_currentTime += Time.deltaTime;
        m_uiMainMenu.SetCurtainAlpha(Mathf.Clamp01(m_currentTime / k_maxFadeTime));
        if (m_currentTime > k_maxFadeTime)
        {
            EndDismissingState();
            SceneManager.LoadScene("Game");
        }
    }
}
