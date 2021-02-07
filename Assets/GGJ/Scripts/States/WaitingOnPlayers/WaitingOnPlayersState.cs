using PersonalFramework;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class WaitingOnPlayersState : FlowStateBase
{
    private WaitingOnPlayersUI m_waitingUI = null;
    private Image m_curtain;
    private float m_remainingTime;
    private float m_maxFadeTime = 1;

    protected override void StartPresentingState()
    {
        m_waitingUI.SetStartButtonActive(NetworkPlayerStatus.s_isHost);
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
    }

    protected override void FixedUpdateActiveState()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount == 5 || GameObject.FindObjectOfType<NetworkedPlayerInfo>() != null) //if networkedPlayerInfo != null, match has started
        {
            ControllingStateStack.ChangeState(new BaseGameState());
        }
        else 
        { 
            m_waitingUI.SetMessage($"Waiting on more players. There are currently {playerCount} players in the lobby out of 5");
        }

        if(NetworkPlayerStatus.s_isHost == false && playerCount == 1)
        {
            NetworkPlayerStatus.s_isHost = true;
            m_waitingUI.SetStartButtonActive(true);
        }
    }
    
    protected override void UpdateDismissingState()
    {
        m_remainingTime -= Time.deltaTime;
        m_curtain.color = new Color(0, 0, 0, Mathf.Clamp01(m_remainingTime / m_maxFadeTime));
        if (m_remainingTime < 0)
        {
            EndDismissingState();
        }
    }

    protected override void HandleMessage(object message)
    {
        if(message is string msg && msg == "ForceStart")
        {
            m_remainingTime = m_maxFadeTime;
            ControllingStateStack.ChangeState(new BaseGameState());
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_waitingUI = Object.FindObjectOfType<WaitingOnPlayersUI>();
        m_ui = m_waitingUI;
        return m_ui != null;
    }
}
