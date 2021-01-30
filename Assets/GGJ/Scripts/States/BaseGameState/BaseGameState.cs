using PersonalFramework;
using UnityEngine;
using Photon.Realtime;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private Transform m_player;
    private Camera m_playerCamera;
    private Transform m_playerCameraTrans;
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
        m_playerCamera = playerCamera;
        m_playerCameraTrans = playerCamera.transform;
        m_inputKeys = InputKeyManagement.GetSavedOrDefaultKeyCodes();

        m_positionMono = player.GetComponent<PositionMono>();
        m_cameraRotation = m_playerCameraTrans.eulerAngles;

        //TODO: Decide if seeker or hider
        m_localPlayerData = Resources.Load<PlayerData>("PlayerData/HiderData");
    }

    protected override void StartActiveState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_teleportManager.Initialise(m_positionMono);
    }

    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        m_playerMovementState = PlayerMovement.GetMovementState(m_playerMovementState, m_inputKeys);

        float deltaTime = Time.deltaTime;
        float speedModifier = m_powerUpData.m_type == PowerUpTypes.SPEED_BOOST ? m_powerUpData.m_affectingValue : 1.0f;
        PlayerMovement.MovePlayer(m_player, m_playerCameraTrans, input, m_localPlayerData, m_playerMovementState, m_positionMono, speedModifier, deltaTime);
        CameraSystem.UpdateCameraRotation(m_playerCameraTrans, ref m_cameraRotation);
        UpdatePowerUps(deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            var prefab = Resources.Load<GameObject>("Ball");
            var ball = GameObject.Instantiate(prefab);
            ball.transform.position = m_player.position + Vector3.up + m_playerCamera.transform.forward * 1.5f;
            ball.GetComponent<Rigidbody>().AddForce(m_playerCamera.transform.forward * 125);
        }
        
    }

    private void UpdatePowerUps(float deltaTime)
    {
        var powerUpsCollided = PowerUpSystem.GetIntersectingPowerUp(m_player, PlayerMovement.GetCurrentHeight(m_playerMovementState, m_localPlayerData));

        if (powerUpsCollided.hitPowerUp)
        {
            PowerUpSystem.TransitionPowerUps(m_powerUpData, powerUpsCollided.powerUpData, m_player);
            m_powerUpData = powerUpsCollided.powerUpData;
            return;
        }

        m_powerUpData = PowerUpSystem.UpdatePowerUpData(m_powerUpData, m_player, deltaTime);
    }

    #region Photon
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected From Server: {cause}");
        m_playerCameraTrans.SetParent(null);
        ControllingStateStack.ChangeState(new ErrorState($"Disconnected from the server MSG: {cause}."));
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room.");
    }

    #endregion
}
