using PersonalFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIStateBase
{
    private Image m_curtain;
    protected override void OnAwake()
    {
        m_curtain = GameObject.Find("Curtain").GetComponent<Image>();
    }

    public void SetCurtainAlpha(float alpha)
    {
        m_curtain.color = new Color(0 ,0, 0, alpha);
    }
}
