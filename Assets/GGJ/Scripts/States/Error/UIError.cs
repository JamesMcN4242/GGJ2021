using PersonalFramework;
using TMPro;

public class UIError : UIStateBase
{
    private TMP_Text m_message;

    protected override void OnAwake()
    {
        m_message = gameObject.FindChildByName("Message").GetComponent<TMP_Text>();
    }

    public void ShowReconnect(bool show)
    {
        gameObject.FindChildByName("Reconnect").SetActive(show);
    }
    
    public void SetMessage(string message)
    {
        m_message.text = message;
    }
}
