using System.Collections.Generic;
using System.Text.RegularExpressions;
using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameDirector : LocalDirector, IConnectionCallbacks, IMatchmakingCallbacks
{
    [RuntimeInitializeOnLoadMethod]
    private static void StartDirector()
    {
        GameObject director = new GameObject("GameDirector");
        GameDirector gameDirector = director.AddComponent<GameDirector>();
        DontDestroyOnLoad(director);
    }

    private void Start()
    {
        m_stateController.PushState(new ConnectingState());
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
    #endregion

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
}