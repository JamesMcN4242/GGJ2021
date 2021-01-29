using PersonalFramework;
using Photon.Pun;
using UnityEngine;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;

    public BaseGameState()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    protected override void StartPresentingState()
    {
        m_inputKeys = Resources.Load<InputKeys>("InputKeys").m_keyCodes;
    }

    protected override void UpdateActiveState()
    {
        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        const float movementSpeed = 4.0f;
        PlayerMovement.MovePlayer(Camera.main.transform, Camera.main.transform, input, movementSpeed, Time.deltaTime);
        CameraSystem.UpdateCameraRotation(Camera.main.transform);
    }
}
