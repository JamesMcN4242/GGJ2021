using System.Text;
using PersonalFramework;
using UnityEngine;

public class UIConnecting : UIStateBase
{
    private const string k_titleText = "Connecting";
    public float m_cycleLength = 0.3f;
    private TMPro.TMP_Text m_message;
    private float m_currentTime = 0;
    private int m_previousCount = 0;

    protected override void OnAwake()
    {
        m_message = gameObject.FindChildByName("Message").GetComponent<TMPro.TMP_Text>();
    }

    public void Reset()
    {
        m_currentTime = 0;
    }

    public void UpdateText()
    {
        m_currentTime += Time.deltaTime;
        m_currentTime %= 1;
        int dotCount = (int) (m_currentTime / 0.3f) + 1;
        if (m_previousCount != dotCount)
        {
            StringBuilder sb = new StringBuilder(k_titleText.Length + 3);
            sb.Append(k_titleText);
            for (int i = 0; i < dotCount; ++i)
            {
                sb.Append(".");
            }

            m_previousCount = dotCount;
            m_message.text = sb.ToString();
        }
        
    }
}
