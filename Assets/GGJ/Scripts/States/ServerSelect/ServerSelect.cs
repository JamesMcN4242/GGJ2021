using System;
using System.Collections.Generic;
using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerSelect : FlowStateBase
{
    private UIServerSelect m_uiServerSelect;
    private float k_maxFadeTime = 1;
    private float m_currentTime = 0;

    private int m_roomCount = 0;

    protected override void StartPresentingState()
    {
        if (PhotonNetwork.IsConnected)
        {
            m_uiServerSelect.OnConnected();    
        }
        else
        {
            m_uiServerSelect.OnDisconnect();
        }

        PhotonNetwork.JoinLobby();
        
        EndPresentingState();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        m_roomCount = roomList.Count;
        m_uiServerSelect.UpdateRoomList(roomList);
    }

    protected override bool AquireUIFromScene()
    {
        m_uiServerSelect = GameObject.Find("UIServerSelect").GetComponent<UIServerSelect>();
        m_ui = m_uiServerSelect;
        Debug.Assert(m_ui != null, "Could no find Server Select screen.");
        return true;
    }
    
    
    protected override void HandleMessage(object message)
    {
        if (CurrentStatus == Status.ACTIVE)
        {
            if (message is string msg)
            {
                if (msg.StartsWith("Room", StringComparison.Ordinal))
                {
                    PhotonNetwork.JoinRoom(msg); // todo: this is actually broke as shit, if 4 rooms exist and room 3 timesout the next room created will be room 4 which will be a duplicate. Need a better way of creating rooms...
                    // Todo: Move to a new state or wait in this state until room joined?
                }
                else
                {
                    switch (msg)
                    {
                        case "CREATE_AND_JOIN":
                            RoomOptions options = new RoomOptions()
                            {
                                IsVisible = true,
                                MaxPlayers = 5,
                                IsOpen = true
                            };
                            PhotonNetwork.JoinOrCreateRoom($"Room {m_roomCount + 1}", options, TypedLobby.Default);
                            break;
                        case "BACK_TO_MENU":
                            ControllingStateStack.ChangeState(new MainMenuState());
                            break;
                    }   
                }
            }   
        }
    }

    protected override void UpdateDismissingState()
    {
        m_currentTime += Time.deltaTime;
        m_uiServerSelect.SetCurtainAlpha(Mathf.Clamp01(m_currentTime / k_maxFadeTime));
        if (m_currentTime > k_maxFadeTime)
        {
            EndDismissingState();
            SceneManager.LoadScene("Game");
        }
    }

    #region Photon
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        m_uiServerSelect.OnConnected();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        string msg = $"Disconnected From Server: {cause}";
        Debug.Log(msg);
        m_uiServerSelect.OnDisconnect();
        PhotonNetwork.Reconnect();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left lobby");
    }

    #endregion
}
