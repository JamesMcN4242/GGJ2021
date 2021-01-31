using PersonalFramework;
using Photon.Pun;
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
    
    private bool m_ballHeld;
    private Rigidbody m_ball;
    private float m_catchBallTimer = 0;
    private Transform m_ballAttachTransform;

    private Vector3 m_cameraRotation;
    private Vector3 m_playerRotation;
    private bool Connected => m_player != null;

    public BaseGameState(GameObject player, Camera playerCamera)
    {
        m_player = player.transform;
        m_playerCamera = playerCamera;
        m_playerCameraTrans = playerCamera.transform;
        m_inputKeys = InputKeyManagement.GetSavedOrDefaultKeyCodes();

        m_positionMono = player.GetComponent<PositionMono>();
        m_cameraRotation = m_playerCameraTrans.localEulerAngles;
        m_playerRotation = m_player.eulerAngles;

        m_ballAttachTransform = m_player.gameObject.FindChildByName("Ball_Attach").transform;
        
        //TODO: Decide if seeker or hider
        m_localPlayerData = Resources.Load<PlayerData>("PlayerData/HiderData");
    }

    protected override void StartActiveState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_teleportManager.Initialise(m_positionMono);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            var ball = PhotonNetwork.Instantiate("Ball",m_player.position + Vector3.up + m_playerCamera.transform.forward * 1.5f, Quaternion.identity);
            m_ball = ball.GetComponent<Rigidbody>();
            ball.transform.SetParent(m_ballAttachTransform,true);
            ball.transform.localPosition = Vector3.zero;
            m_ballHeld = true;
            m_ball.isKinematic = true;
        }
    }

    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        m_playerMovementState = PlayerMovement.GetMovementState(m_playerMovementState, m_inputKeys);

        float deltaTime = Time.deltaTime;
        float speedModifier = m_powerUpData.m_type == PowerUpTypes.SPEED_BOOST ? m_powerUpData.m_affectingValue : 1.0f;
        PlayerMovement.MovePlayer(m_player, m_playerCameraTrans, input, m_localPlayerData, m_playerMovementState, m_positionMono, speedModifier, deltaTime);
        CameraSystem.UpdateCameraRotation(m_player,ref m_playerRotation,m_playerCameraTrans, ref m_cameraRotation);
        UpdatePowerUps(deltaTime);

        if (Input.GetMouseButton(0) && m_ballHeld)
        {
            m_ball.isKinematic = false;
            m_ball.AddForce(m_playerCamera.transform.forward * 130);
            m_ball.transform.SetParent(null,true);
            m_ballHeld = false;
            m_catchBallTimer = 2f;
        }

        if (m_catchBallTimer > 0)
        {
            m_catchBallTimer -= Time.deltaTime;
        }

        if (m_ballHeld == false && m_catchBallTimer < 0 && m_ball != null && Vector3.Distance(m_ball.transform.position,m_player.position) < 1.5f)
        {
            m_ball.transform.SetParent(m_ballAttachTransform,true);
            m_ball.transform.localPosition = Vector3.zero;
            m_ballHeld = true;
            m_ball.velocity = Vector3.zero;
            m_ball.isKinematic = true;
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
