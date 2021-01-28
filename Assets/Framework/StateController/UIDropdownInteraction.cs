////////////////////////////////////////////////////////////
/////   UIDropdownInteraction.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PersonalFramework
{
    [RequireComponent(typeof(Selectable))]
    public class UIDropdownInteraction : WatchedObject
    {
        public string m_message = string.Empty;

        ~UIDropdownInteraction()
        {
            ClearAllObservers();
        }

        // Start is called before the first frame update
        void Start()
        {
            Selectable selectable = GetComponent<Selectable>();

            if (selectable is Dropdown)
            {
                Dropdown dropdown = selectable as Dropdown;
                dropdown.onValueChanged.AddListener(OnNewValue);
            }
            else if(selectable is TMP_Dropdown)
            {
                TMP_Dropdown dropdown = selectable as TMP_Dropdown;
                dropdown.onValueChanged.AddListener(OnNewValue);
            }
            else
            {
                Debug.LogError($"No way to use selectable object of type {selectable.GetType()} as a dropdown");
            }
        }

        private void OnNewValue(int newIndex)
        {
            NotifyObservers(m_message + newIndex);
        }
    }
}