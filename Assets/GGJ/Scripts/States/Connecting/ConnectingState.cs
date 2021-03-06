﻿using PersonalFramework;
using Photon.Pun;
using UnityEngine.Assertions;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectingState : FlowStateBase
{
    private Image m_curtain;
    private float m_remainingTime;
    private float m_maxFadeTime = 1;
    private UIConnecting m_uiConnecting;
    
    protected override bool AquireUIFromScene()
    {
        m_uiConnecting = GameObject.Find("UIConnecting").GetComponent<UIConnecting>();
        m_ui = m_uiConnecting;
        return true;
    }

    protected override void StartPresentingState()
    {
        if(!PhotonNetwork.IsConnected)
        {
            bool connected = PhotonNetwork.ConnectUsingSettings();
            Assert.IsTrue(connected, "Can't Connect to photon!");
        }

        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
        m_curtain.color = Color.black;
    }



    protected override void UpdateActiveState()
    {
        m_uiConnecting.UpdateText();
        if(PhotonNetwork.IsConnected)
        {
            ControllingStateStack.ChangeState(new WaitingOnPlayersState());
        }
    }

    protected override void UpdateDismissingState()
    {
        m_remainingTime -= Time.deltaTime;
        m_curtain.color = new Color(0, 0, 0, Mathf.Clamp01(m_remainingTime / m_maxFadeTime));
        if (m_remainingTime < 0)
        {
            EndDismissingState();
        }
    }

    #region Photon
    
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");
        m_remainingTime = m_maxFadeTime;

        ControllingStateStack.ChangeState(new WaitingOnPlayersState());
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        string msg = $"Disconnected From Server: {cause}";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }
    
    public override void OnLeftRoom()
    {
        string msg = $"Error: Unexpectedly Left Room: {PhotonNetwork.CurrentRoom.Name}.";
        Debug.Log(msg);
        ControllingStateStack.ChangeState(new ErrorState(msg));
    }

    #endregion
}
