using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuState : FlowStateBase
{
    private UIMainMenu m_uiMainMenu;
    private float k_maxFadeTime = 1;
    private float m_currentTime;

    protected override void StartPresentingState()
    {
        if (PhotonNetwork.IsConnected)
        {
            m_uiMainMenu.OnConnected();
        }
        else
        {
            m_uiMainMenu.OnDisconnect();    
        }

        m_currentTime = 0;
        EndPresentingState();
    }

    protected override void UpdatePresentingState()
    {
        m_currentTime += Time.deltaTime;
        m_uiMainMenu.SetCurtainAlpha(Mathf.Clamp01(1f - m_currentTime / k_maxFadeTime));
        if (m_currentTime / k_maxFadeTime > 1f)
        {
            EndPresentingState();
        }
    }

    protected override void StartActiveState()
    {
        Debug.Log("Initialising Connection with Photon!");
        if (PhotonNetwork.IsConnected == false)
        {
            bool connected = PhotonNetwork.ConnectUsingSettings();
            Debug.Assert(connected, "Can't Connect to photon!"); // todo: check this may be a bad idea to assert    
        }
    }

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
                        ControllingStateStack.ChangeState(new ServerSelect());
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
        }
    }

    #region Photon
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        m_uiMainMenu.OnConnected();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        string msg = $"Disconnected From Server: {cause}";
        Debug.Log(msg);
        m_uiMainMenu.OnDisconnect();
        PhotonNetwork.Reconnect();
    }

    #endregion
}
