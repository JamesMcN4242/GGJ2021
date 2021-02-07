using System;
using System.Collections.Generic;
using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerSelect : FlowStateBase
{
    private UIServerSelect m_uiServerSelect;
    private float k_maxFadeTime = 1;
    private float m_currentTime = 0;
    private Image m_curtain;

    enum NextState
    {
        MENU,
        GAME,
    }

    private NextState m_nextState;
    
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

        m_curtain = m_uiServerSelect.transform.parent.Find("Curtain").GetComponent<Image>();

        PhotonNetwork.JoinLobby();
    }

    protected override void UpdatePresentingState()
    {
        m_currentTime += Time.deltaTime;
        m_curtain.color = m_curtain.color.CopyWithA(1 - (m_currentTime / k_maxFadeTime));
        if(m_currentTime/k_maxFadeTime > 1f)
            EndPresentingState();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        m_uiServerSelect.UpdateRoomList(roomList);
        RebuildObserverList();
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
                Debug.Log(msg);
                if (msg.StartsWith("Room", StringComparison.Ordinal))
                {
                    PhotonNetwork.JoinRoom(msg);
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
                            PhotonNetwork.JoinOrCreateRoom($"Room {System.Guid.NewGuid()}", options, TypedLobby.Default);
                            NetworkPlayerStatus.s_isHost = true;
                            break;
                        case "BACK_TO_MENU":
                            ControllingStateStack.ChangeState(new MainMenuState());
                            m_nextState = NextState.MENU;
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
            if(m_nextState == NextState.GAME)
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

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");
        m_currentTime = k_maxFadeTime;
        EndActiveState();
        m_nextState = NextState.GAME;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        string msg = $"Join Room Failed with code: {returnCode}\n{message}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string msg = $"Join Room Failed with code: {returnCode}\n{message}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));;
    }
    
    public override void OnLeftRoom()
    {
        string msg = $"Error: Unexpectedly Left Room: {PhotonNetwork.CurrentRoom.Name}.";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }
    
    #endregion
}
