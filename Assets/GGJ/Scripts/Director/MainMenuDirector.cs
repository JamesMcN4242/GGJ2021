using System.Collections.Generic;
using NUnit.Framework;
using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuDirector : LocalDirector, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    private void Start()
    {
        m_stateController.PushState(new MainMenuState());
    }
    
    public virtual void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public virtual void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    #region Photon
    public void OnConnected()
    {
        m_stateController.OnConnected();
    }

    public void OnConnectedToMaster()
    {
        m_stateController.OnConnectedToMaster();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        m_stateController.OnDisconnected(cause);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        m_stateController.OnRegionListReceived(regionHandler);
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        m_stateController.OnCustomAuthenticationResponse(data);
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        m_stateController.OnCustomAuthenticationFailed(debugMessage);
    }


    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
        m_stateController.OnCreatedRoom();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        m_stateController.OnCreateRoomFailed(returnCode,message);
    }

    public void OnJoinedRoom()
    {
        m_stateController.OnJoinedRoom();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        m_stateController.OnJoinRandomFailed(returnCode,message);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        m_stateController.OnJoinRandomFailed(returnCode,message);
    }

    public void OnLeftRoom()
    {
        m_stateController.OnLeftRoom();
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion

    public void OnJoinedLobby()
    {
        m_stateController.OnJoinedLobby();
    }

    public void OnLeftLobby()
    {
        m_stateController.OnLeftLobby();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        m_stateController.OnRoomListUpdate(roomList);
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }
}
