using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkedPlayerInfo : MonoBehaviour, IPunObservable
{
    [Serializable]
    public struct PlayerInformation
    {
        public int m_playerId;
        public bool m_isSeeker;
        public string m_spawnPos;
    }

    public PlayerInformation[] m_playerInformations;

    public void SetUpFromPlayers()
    {
        var players = PhotonNetwork.CurrentRoom.Players;
        m_playerInformations = new PlayerInformation[players.Count];
        int index = 0;
        bool seekerAssigned = false;
        List<HiderSpawnerMono> hiderSpawns = new List<HiderSpawnerMono>(GameObject.FindObjectsOfType<HiderSpawnerMono>());

        foreach (var keyVal in players)
        {
            bool isSeeker = !seekerAssigned && (Random.Range(0, 1) == 0 || players.Count == index + 1);
            seekerAssigned |= isSeeker;

            MonoBehaviour spawn = null;
            if (isSeeker)
            {
                spawn = GameObject.FindObjectOfType<SeekerSpawnerMono>();
            }
            else
            { 
                spawn = hiderSpawns[Random.Range(0, hiderSpawns.Count)];
                hiderSpawns.Remove(spawn as HiderSpawnerMono);
            }


            m_playerInformations[index] = new PlayerInformation()
            {
                m_isSeeker = isSeeker,
                m_playerId = keyVal.Value.ActorNumber,
                m_spawnPos = spawn.name
            };
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_playerInformations);
        }
        else
        {
            m_playerInformations = (PlayerInformation[])stream.ReceiveNext();
        }
    }
}
