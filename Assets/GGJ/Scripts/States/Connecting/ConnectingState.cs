using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectingState : FlowStateBase
{
    private Image m_curtain;
    private float m_remainingTime;
    private float m_maxFadeTime = 1;
    private UIConnecting m_uiConnecting;
    
    protected override bool AquireUIFromScene()
    {
        m_uiConnecting = GameObject.Find("UIConnecting").GetComponent<UIConnecting>();
        m_ui = m_uiConnecting;
        return true;
    }

    protected override void StartPresentingState()
    {
        bool connected = PhotonNetwork.ConnectUsingSettings();
        Assert.IsTrue(connected, "Can't Connect to photon!");
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
        m_curtain.color = Color.black;
        EndPresentingState();
    }

    protected override void UpdateActiveState()
    {
        m_uiConnecting.UpdateText();
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

    #region Photon
    public override void OnConnected()
    {
        Debug.Log("OnConnected() was called by PUN.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");

        RoomOptions options = new RoomOptions()
        {
            IsVisible = false,
            IsOpen = true,
            MaxPlayers = 6
        };
        PhotonNetwork.JoinOrCreateRoom("TestRoom", options, TypedLobby.Default);
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created.");
        SpawnSystem.SpawnPowerUps();
        NetworkPlayerStatus.s_isHost = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");
        m_remainingTime = m_maxFadeTime;

        ControllingStateStack.ChangeState(new WaitingOnPlayersState());
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        string msg = $"Join Room Failed with code: {returnCode}\n{message}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string msg = $"Join Room Failed with code: {returnCode}\n{message}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        string msg = $"Disconnected From Server: {cause}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }
    
    public override void OnLeftRoom()
    {
        string msg = $"Error: Unexpectedly Left Room: {PhotonNetwork.CurrentRoom.Name}.";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }

    #endregion
}
