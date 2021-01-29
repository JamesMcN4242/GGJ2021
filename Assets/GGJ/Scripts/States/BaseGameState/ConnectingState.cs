using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectingState : FlowStateBase
{
    private GameObject m_player;
    private Camera m_playerCamera;
    private Image m_curtain;
    private float m_remainingTime;
    private float m_maxFadeTime = 1;
    
    protected override void StartPresentingState()
    {
        bool connected = PhotonNetwork.ConnectUsingSettings();
        Assert.IsTrue(connected, "Can't Connect to photon!");
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
        m_curtain.color = Color.black;
        EndPresentingState();
    }

    protected override void UpdateDismissingState()
    {
        m_remainingTime -= Time.deltaTime;
        m_curtain.color = new Color(0, 0, 0, m_remainingTime / m_maxFadeTime);
        if (m_remainingTime < 0)
        {
            ControllingStateStack.PushState(new BaseGameState(m_player,m_playerCamera));
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected From Server: {cause}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");
        
        m_player = PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity);
        m_player.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        ControllingStateStack.PopState(this);
        m_remainingTime = m_maxFadeTime;
        var attachPoint = m_player.FindChildByName("Camera_Attach").transform;
        m_playerCamera = Camera.main;
        var transform = m_playerCamera.transform;
        transform.SetParent(attachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Failed with code: {returnCode}\n{message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Failed with code: {returnCode}\n{message}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room.");
    }

    #endregion
}
