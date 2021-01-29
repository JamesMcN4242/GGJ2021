using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private GameObject m_player;
    private Camera m_playerCamera;
    private PlayerData m_localPlayerData;
    private Vector3 m_cameraRotation;

    private bool Connected => m_localPlayerData != null;

    public BaseGameState(GameObject player, Camera playerCamera)
    {
        m_player = player;
        m_playerCamera = playerCamera;
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
