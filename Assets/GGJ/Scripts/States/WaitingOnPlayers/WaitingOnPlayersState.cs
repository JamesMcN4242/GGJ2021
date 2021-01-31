using PersonalFramework;
using Photon.Pun;
using UnityEngine;

public class WaitingOnPlayersState : FlowStateBase
{
    private WaitingOnPlayersUI m_waitingUI = null;

    protected override void StartPresentingState()
    {
        m_waitingUI.SetStartButtonActive(NetworkPlayerStatus.s_isHost);
    }

    protected override void FixedUpdateActiveState()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount == 5)
        {
            ControllingStateStack.ChangeState(new BaseGameState());
        }
        else 
        { 
            m_waitingUI.SetMessage($"Waiting on more players. There are currently {playerCount} players in the lobby out of 5");
        }
    }

    protected override void HandleMessage(object message)
    {
        if(message is string msg && msg == "ForceStart")
        {
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
