using System.Collections.Generic;
using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class BaseGameState : FlowStateBase
{
    protected override void StartPresentingState()
    {
        bool connected = PhotonNetwork.ConnectUsingSettings();
        Assert.IsTrue(connected,"Can't Connect to photon!");
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
