////////////////////////////////////////////////////////////
/////   LocalDirector.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;

namespace PersonalFramework
{
    public class LocalDirector : MonoBehaviour
    {
        protected StateController m_stateController = new StateController();

        // Update is called once per frame
        void Update()
        {
            m_stateController.UpdateStack();
        }

        void FixedUpdate()
        {
            m_stateController.FixedUpdateStack();
        }
    }
}