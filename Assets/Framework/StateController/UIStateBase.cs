////////////////////////////////////////////////////////////
/////   UIStateBase.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;

namespace PersonalFramework
{
    public class UIStateBase : MonoBehaviour
    {
        private UIContent m_content = null;

        void Awake()
        {
            m_content = transform.GetComponentInChildren<UIContent>(true);
            OnAwake();
            SetContentActiveStatus(false);
        }

        protected virtual void OnAwake()
        {
        }

        public void SetContentActiveStatus(bool enabled)
        {
            if (m_content != null)
            {
                m_content.gameObject.SetActive(enabled);
            }
        }
    }
}