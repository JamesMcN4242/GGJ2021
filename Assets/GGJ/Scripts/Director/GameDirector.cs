using System.Collections.Generic;
using System.Text.RegularExpressions;
using PersonalFramework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameDirector : LocalDirector, IConnectionCallbacks
{
    [RuntimeInitializeOnLoadMethod]
    private static void StartDirector()
    {
        GameObject director = new GameObject("GameDirector");
        GameDirector gameDirector = director.AddComponent<GameDirector>();
        DontDestroyOnLoad(director);
    }

    private void Start()
    {
        m_stateController.PushState(new BaseGameState());
    }
    
    public virtual void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public virtual void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    #region Photon
    public void OnConnected()
    {
        m_stateController.OnConnected();
    }

    public void OnConnectedToMaster()
    {
        m_stateController.OnConnectedToMaster();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        m_stateController.OnDisconnected(cause);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        m_stateController.OnRegionListReceived(regionHandler);
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        m_stateController.OnCustomAuthenticationResponse(data);
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        m_stateController.OnCustomAuthenticationFailed(debugMessage);
    }
    #endregion
}