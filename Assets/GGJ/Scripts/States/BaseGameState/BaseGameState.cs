using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;

    protected override void StartPresentingState()
    {
        bool connected = PhotonNetwork.ConnectUsingSettings();
        Assert.IsTrue(connected, "Can't Connect to photon!");
        m_inputKeys = Resources.Load<InputKeys>("InputKeys").m_keyCodes;
    }

    protected override void UpdateActiveState()
    {
        const float movementSpeed = 8.0f;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        PlayerMovement.MovePlayer(Camera.main.transform, Camera.main.transform, input, movementSpeed, Time.deltaTime);
        CameraSystem.UpdateCameraRotation(Camera.main.transform);
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
        
        var cube = PhotonNetwork.Instantiate("Cube",Vector3.zero,Quaternion.identity);
        cube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
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
