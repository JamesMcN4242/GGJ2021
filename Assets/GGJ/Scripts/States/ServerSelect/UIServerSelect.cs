using System.Collections.Generic;
using PersonalFramework;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UIServerSelect : UIStateBase
{
    private Image m_curtain;
    public static readonly Color k_connectedCol = new Color(0.4f,0.8f,0.4f);
    public static readonly Color k_disconnectedCol = new Color(0.8f, 0.25f, 0.18f);

    private Image m_connectedIcon;
    private TMPro.TMP_Text m_connectionStatus;

    private Transform m_inactiveRoomParent;
    private Transform m_scrollViewContent;
    private GameObject m_roomSelectPrefab;

    protected override void OnAwake()
    {
        m_roomSelectPrefab = Resources.Load<GameObject>("Room Select");
        
        m_inactiveRoomParent = gameObject.FindChildByName("Inactive RoomList").transform;
        m_scrollViewContent = gameObject.FindChildByName("Content").transform;
        
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
        var statusGo = gameObject.FindChildByName("Connected Status");
        m_connectedIcon = statusGo.FindChildByName("Icon").GetComponent<Image>();
        m_connectionStatus = statusGo.FindChildByName("Text").GetComponent<TMPro.TMP_Text>();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        m_scrollViewContent.gameObject.SetActive(false);
        int diff = m_scrollViewContent.childCount - roomList.Count;
        if (diff < 0)
        {
            var go = Instantiate(m_roomSelectPrefab);
            m_scrollViewContent.SetParent(go.transform);
        }
        else if(diff > 0)
        {
            int childCount = m_scrollViewContent.childCount;
            for (int i = childCount - 1; i >= childCount - diff; --i)
            {
                Transform transform = m_scrollViewContent.GetChild(i);
                transform.SetParent(m_inactiveRoomParent);
            }
        }

        for (int i = 0; i < roomList.Count; ++i)
        {
            var transform = m_scrollViewContent.GetChild(i);
            transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"{roomList[i].Name} {roomList[i].PlayerCount}/{roomList[i].MaxPlayers}";
            transform.GetChild(1).GetComponent<UIButtonInteraction>().m_message = roomList[i].Name;
        }

        m_scrollViewContent.gameObject.SetActive(true);
    }

    public void SetCurtainAlpha(float alpha)
    {
        m_curtain.color = m_curtain.color.CopyWithA(alpha);
    }

    public void OnConnected()
    {
        m_connectionStatus.text = "Connected";
        m_connectedIcon.color = k_connectedCol;
    }
    
    public void OnDisconnect()
    {
        m_connectionStatus.text = "Disconnected";
        m_connectedIcon.color = k_disconnectedCol;
    }
}