using PersonalFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameUI : UIStateBase
{
    private TextMeshProUGUI m_powerUpSeconds = null;
    private Image m_powerUpBacking = null;
    private TextMeshProUGUI m_doorButtonsClicked = null;

    protected override void OnAwake()
    {
        m_powerUpSeconds = gameObject.GetComponentFromChild<TextMeshProUGUI>("PowerUpTimer");
        m_powerUpBacking = gameObject.GetComponentFromChild<Image>("PowerUpFill");
        m_doorButtonsClicked = gameObject.GetComponentFromChild < TextMeshProUGUI>("DoorButtonClicks");
    }

    public void UpdateDoorButtonText(int buttonsclicked)
    {
        m_doorButtonsClicked.text = $"Door buttons clicked: {buttonsclicked}/4"; 
    }

    public void UpdatePowerUpBacking(float timeRemaining, float maxPowerUpTime)
    {
        float fill = timeRemaining / maxPowerUpTime;
        if(fill != m_powerUpBacking.fillAmount)
        {
            m_powerUpBacking.fillAmount = fill;
            m_powerUpSeconds.text = timeRemaining > 0.3f ? Mathf.CeilToInt(timeRemaining).ToString() : string.Empty;
        }
    }
}
