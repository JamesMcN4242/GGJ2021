using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private Transform m_player;
    private Transform m_playerCamera;
    private PlayerData m_localPlayerData;

    private Vector3 m_cameraRotation;

    private bool Connected => m_player != null;

    public BaseGameState(GameObject player, Camera playerCamera)
    {
        m_player = player.transform;
        m_playerCamera = playerCamera.transform;
        m_inputKeys = Resources.Load<InputKeys>("InputKeys").m_keyCodes;

        m_cameraRotation = m_playerCamera.eulerAngles;

        //TODO: Decide if seeker or hider
        m_localPlayerData = Resources.Load<PlayerData>("PlayerData/HiderData");
    }
    
    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        PlayerMovement.MovePlayer(m_player, m_playerCamera, input, m_localPlayerData.m_playerSpeed, Time.deltaTime);
        CameraSystem.UpdateCameraRotation(m_playerCamera, ref m_cameraRotation);
    }

    #region Photon
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected From Server: {cause}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room.");
    }

    #endregion
}
