using System;
using PersonalFramework;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class BaseGameState : FlowStateBase
{
    private KeyCodeSet m_inputKeys;
    private Transform m_player;
    private CharacterController m_characterController;
    private Camera m_playerCamera;
    private Transform m_playerCameraTrans;
    private PositionMono m_positionMono;
    private PlayerData m_localPlayerData;
    private PlayerMovement.MovementState m_playerMovementState;
    private TeleportManager m_teleportManager = new TeleportManager();
    private PowerUpData m_powerUpData;
    private bool m_isSeeker;
    
    private bool m_ballHeld;
    private Rigidbody m_ball;
    private float m_catchBallTimer = 0;
    private Transform m_ballAttachTransform;

    private Vector3 m_cameraRotation;
    private Vector3 m_playerRotation;
    private bool Connected => m_player != null;

    private float m_separationTime = 0;

    private LoserCount m_loserCount;

    public BaseGameState()
    {
        m_inputKeys = InputKeyManagement.GetSavedOrDefaultKeyCodes();
        if(NetworkPlayerStatus.s_isHost)
        {
            GameObject obj = PhotonNetwork.Instantiate("NetworkedPlayerInfo", Vector3.zero, Quaternion.identity);
            obj.GetComponent<NetworkedPlayerInfo>().SetUpFromPlayers();
        }
    }

    protected override void StartPresentingState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void UpdatePresentingState()
    {
        NetworkedPlayerInfo playerInfo = GameObject.FindObjectOfType<NetworkedPlayerInfo>();
        if (playerInfo != null)
        {
            //TODO: Player model/colour differences depending on seeker or not
            int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
            var playerInformation = System.Array.Find(playerInfo.m_playerInformations.m_playerInformation, entry => entry.m_playerId == playerId);

            if(playerInformation.m_playerId != playerId)
            {
                //Default values to set
                int randomSpawnNum = Random.Range(0, 4);
                playerInformation = new NetworkedPlayerInfo.PlayerInformation() { m_playerId = playerId, m_isSeeker = false, m_spawnPos = $"Hider ({randomSpawnNum})" };
            }

            Vector3 startPos = GameObject.Find(playerInformation.m_spawnPos).transform.position;
            GameObject player = PhotonNetwork.Instantiate("Player", startPos, Quaternion.identity);
            m_characterController = player.GetComponent<CharacterController>();
            player.FindChildByName("Knight").SetActive(false);

            Camera playerCamera = Camera.main;
            var transform = playerCamera.transform;

            var attachPoint = player.FindChildByName("Camera_Attach").transform;
            transform.SetParent(attachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            m_player = player.transform;
            m_playerCamera = playerCamera;
            m_playerCameraTrans = playerCamera.transform;
            m_positionMono = player.GetComponent<PositionMono>();
            m_cameraRotation = m_playerCameraTrans.localEulerAngles;
            m_playerRotation = m_player.eulerAngles;

            m_ballAttachTransform = m_player.gameObject.FindChildByName("Ball_Attach").transform;

            m_isSeeker = playerInformation.m_isSeeker;
            string dataName = playerInformation.m_isSeeker ? "Seeker" : "Hider";
            m_localPlayerData = Resources.Load<PlayerData>($"PlayerData/{dataName}Data");

            m_positionMono.IsSeeker = m_isSeeker;

            EndPresentingState();
        }
    }

    protected override void StartActiveState()
    {

        if ( m_isSeeker )
        {
            var ball = GameObject.Find("Ball");
            m_ball = ball.GetComponent<Rigidbody>();
            ball.GetComponent<PhotonView>().RequestOwnership();
            ball.transform.SetParent(m_ballAttachTransform,true);
            ball.transform.localPosition = Vector3.zero;
            m_ballHeld = true;
            m_ball.isKinematic = true;
        }
        
        m_teleportManager.Initialise(m_positionMono);

        m_loserCount = GameObject.Find("LosersBox").GetComponent<LoserCount>();
    }

    protected override void UpdateActiveState()
    {
        if (Connected == false) return;

        Vector2 input = PlayerMovement.GetPlayerMovement(m_inputKeys);
        m_playerMovementState = PlayerMovement.GetMovementState(m_playerMovementState, m_inputKeys);

        float deltaTime = Time.deltaTime;
        float speedModifier = m_powerUpData.m_type == PowerUpTypes.SPEED_BOOST ? m_powerUpData.m_affectingValue : 1.0f;
        PlayerMovement.MovePlayer(m_characterController, m_player, m_playerCameraTrans, input, m_localPlayerData, m_playerMovementState, m_positionMono, speedModifier, deltaTime);
        CameraSystem.UpdateCameraRotation(m_player,ref m_playerRotation,m_playerCameraTrans, ref m_cameraRotation);
        LeverSystem.UpdateLeverInteractions(m_player, PlayerMovement.GetCurrentHeight(m_playerMovementState, m_localPlayerData), m_inputKeys);
        UpdatePowerUps(deltaTime);

        if (m_isSeeker)
        {
            if (Input.GetMouseButton(0) && m_ballHeld)
            {
                m_ball.isKinematic = false;
                m_ball.AddForce(m_playerCamera.transform.forward * 130);
                m_ball.transform.SetParent(null,true);
                m_ballHeld = false;
                m_catchBallTimer = 0.8f;
                m_separationTime = 0;
            }

            if (m_ballHeld == false)
            {
                m_separationTime += Time.deltaTime;
                var diff = m_player.position - m_ball.position;
                if (diff.magnitude < 5f)
                {
                    m_ball.AddForce(diff.normalized * 0.5f);
                }

                if (m_separationTime > 10f)
                {
                    m_ball.velocity -= m_ball.velocity * 0.9f * Time.deltaTime;
                }    
            }
            
            if (m_catchBallTimer > 0)
            {
                m_catchBallTimer -= Time.deltaTime;
            }

            if (m_ballHeld == false && m_catchBallTimer < 0 && m_ball != null && Vector3.Distance(m_ball.transform.position.CopyWithY(0),m_player.position.CopyWithY(0)) < 1.5f && Mathf.Abs(m_ball.transform.position.y-(m_player.transform.position.y + m_player.GetComponent<CharacterController>().height / 2)) < 1.5f)
            {
                m_ball.transform.SetParent(m_ballAttachTransform,true);
                m_ball.transform.localPosition = Vector3.zero;
                m_ballHeld = true;
                m_ball.velocity = Vector3.zero;
                m_ball.isKinematic = true;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount != 1 && m_loserCount.m_loserCount == PhotonNetwork.CurrentRoom.PlayerCount - 1)
        {
            Debug.Log("Seeker Wins");
            ControllingStateStack.ChangeState(new ErrorState(m_isSeeker ? "Congrats you Win!" : "LOSER!"));
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
