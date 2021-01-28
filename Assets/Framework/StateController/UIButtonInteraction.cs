////////////////////////////////////////////////////////////
/////   UIButtonInteraction.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

namespace PersonalFramework
{
    [RequireComponent(typeof(Button))]
    public class UIButtonInteraction : WatchedObject
    {
        public string m_message = string.Empty;

        ~UIButtonInteraction()
        {
            ClearAllObservers();
        }

        // Start is called before the first frame update
        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            NotifyObservers(m_message);
        }
    }
}