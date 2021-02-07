using PersonalFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIStateBase
{
    private Image m_curtain;
    public static readonly Color k_connectedCol = new Color(0.4f,0.8f,0.4f);
    public static readonly Color k_disconnectedCol = new Color(0.8f, 0.25f, 0.18f);

    private Image m_connectedIcon;
    private TMPro.TMP_Text m_connectionStatus;
    
    protected override void OnAwake()
    {
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
        var statusGo = gameObject.FindChildByName("Connected Status");
        m_connectedIcon = statusGo.FindChildByName("Icon").GetComponent<Image>();
        m_connectionStatus = statusGo.FindChildByName("Text").GetComponent<TMPro.TMP_Text>();
    }

    public void SetCurtainAlpha(float alpha)
    {
        m_curtain.color =m_curtain.color.CopyWithA(alpha);
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
