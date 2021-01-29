using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private PlayerData m_localPlayerData;
    private Vector3 m_cameraRotation;

    private bool Connected => m_localPlayerData != null;

    protected override void StartPresentingState()
    {
        bool connected = PhotonNetwork.ConnectUsingSettings();
        Assert.IsTrue(connected, "Can't Connect to photon!");
        m_inputKeys = Resources.Load<InputKeys>("InputKeys").m_keyCodes;

        m_cameraRotation = Camera.main.transform.eulerAngles;
    }

    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        //Todo: Update to our local device's cube object
        Transform playerTransform = Camera.main.transform;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        PlayerMovement.MovePlayer(playerTransform, Camera.main.transform, input, m_localPlayerData.m_playerSpeed, Time.deltaTime);
        CameraSystem.UpdateCameraRotation(Camera.main.transform, ref m_cameraRotation);
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

        //TODO: Randomly assign this instead of giving it to the first player
        m_localPlayerData = Resources.Load<PlayerData>("PlayerData/SeekerData");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");

        var cube = PhotonNetwork.Instantiate("Cube",Vector3.zero,Quaternion.identity);
        cube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        m_localPlayerData ??= Resources.Load<PlayerData>("PlayerData/HiderData");
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
