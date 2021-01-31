using PersonalFramework;
using UnityEngine;
using TMPro;

public class WaitingOnPlayersUI : UIStateBase
{
    private TextMeshProUGUI m_messageText = null;
    private GameObject m_startButton = null;

    protected override void OnAwake()
    {
        m_messageText = gameObject.GetComponentFromChild<TextMeshProUGUI>("Message");
        m_startButton = gameObject.FindChildByName("ForceStart");

        m_startButton.SetActive(false);
    }

    public void SetMessage(string msg)
    {
        m_messageText.text = msg;
    }

    public void SetStartButtonActive(bool active)
    {
        m_startButton.SetActive(active);
    }
}
