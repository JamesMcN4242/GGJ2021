////////////////////////////////////////////////////////////
/////   UIToggleInteraction.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

namespace PersonalFramework
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleInteraction : WatchedObject
    {
        public string m_message = string.Empty;

        ~UIToggleInteraction()
        {
            ClearAllObservers();
        }

        // Start is called before the first frame update
        void Start()
        {
            Toggle toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggled);
        }

        private void OnToggled(bool toggleState)
        {
            NotifyObservers(m_message + toggleState);
        }
    }
}