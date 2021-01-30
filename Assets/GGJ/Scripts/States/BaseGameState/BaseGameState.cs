using PersonalFramework;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private Transform m_player;
    private Transform m_playerCamera;
    private PositionMono m_positionMono;
    private PlayerData m_localPlayerData;
    private PlayerMovement.MovementState m_playerMovementState;
    private TeleportManager m_teleportManager = new TeleportManager();
    private PowerUpData m_powerUpData;

    private Vector3 m_cameraRotation;
    private bool Connected => m_player != null;

    public BaseGameState(GameObject player, Camera playerCamera)
    {
        m_player = player.transform;
        m_playerCamera = playerCamera.transform;
        m_inputKeys = InputKeyManagement.GetSavedOrDefaultKeyCodes();

        m_positionMono = player.GetComponent<PositionMono>();
        m_cameraRotation = m_playerCamera.eulerAngles;

        //TODO: Decide if seeker or hider
        m_localPlayerData = Resources.Load<PlayerData>("PlayerData/HiderData");
    }

    protected override void StartActiveState()
    {
        m_teleportManager.Initialise(m_positionMono);
    }

    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        m_playerMovementState = PlayerMovement.GetMovementState(m_playerMovementState, m_inputKeys);

        float deltaTime = Time.deltaTime;
        float speedModifier = m_powerUpData.m_type == PowerUpTypes.SPEED_BOOST ? m_powerUpData.m_affectingValue : 1.0f;
        PlayerMovement.MovePlayer(m_player, m_playerCamera, input, m_localPlayerData, m_playerMovementState, m_positionMono, speedModifier, deltaTime);
        CameraSystem.UpdateCameraRotation(m_playerCamera, ref m_cameraRotation);

        var powerUpsCollided = PowerUpSystem.GetIntersectingPowerUp(m_player, PlayerMovement.GetCurrentHeight(m_playerMovementState, m_localPlayerData));
        m_powerUpData = powerUpsCollided.hitPowerUp ? powerUpsCollided.powerUpData : PowerUpSystem.UpdatePowerUpData(m_powerUpData, deltaTime);
    }

    #region Photon
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected From Server: {cause}");
        m_playerCamera.SetParent(null);
        ControllingStateStack.ChangeState(new ErrorState($"Disconnected from the server MSG: {cause}."));
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room.");
    }

    #endregion
}
