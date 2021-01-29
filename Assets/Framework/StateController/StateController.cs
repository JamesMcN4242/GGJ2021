////////////////////////////////////////////////////////////
/////   StateController.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace PersonalFramework
{
    public class StateController
    {
        private Stack<FlowStateBase> m_stateStack = new Stack<FlowStateBase>();

        public void PushState(FlowStateBase state)
        {
            Debug.Assert(m_stateStack.Count == 0 || m_stateStack.Peek() != state, "Trying to push already active state");
            m_stateStack.Push(state);
            state.SetStateController(this);
        }

        public void PopState(FlowStateBase state)
        {
            Debug.Assert(m_stateStack.Count > 0 && m_stateStack.Peek() == state, "Trying to pop non active state");
            m_stateStack.Peek().EndActiveState();
        }

        public void UpdateStack()
        {
            if (m_stateStack.Count > 0)
            {
                FlowStateBase state = m_stateStack.Peek();
                state.UpdateState();
                if (state.IsDismissed())
                {
                    m_stateStack.Pop();
                }
            }
        }

        public void FixedUpdateStack()
        {
            if (m_stateStack.Count > 0)
            {
                FlowStateBase state = m_stateStack.Peek();
                state.FixedUpdateState();
            }
        }
        
        public virtual void OnConnected()
        {
            var state = m_stateStack.Peek();
            state.OnConnected();
        }

        public virtual void OnConnectedToMaster()
        {
            var state = m_stateStack.Peek();
            state.OnConnectedToMaster();
        }

        public virtual void OnDisconnected(DisconnectCause cause)
        {
            var state = m_stateStack.Peek();
            state.OnDisconnected(cause);
        }

        public virtual void OnRegionListReceived(RegionHandler regionHandler)
        {
            var state = m_stateStack.Peek();
            state.OnRegionListReceived(regionHandler);
        }

        public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            var state = m_stateStack.Peek();
            state.OnCustomAuthenticationResponse(data);
        }

        public virtual void OnCustomAuthenticationFailed(string debugMessage)
        {
            var state = m_stateStack.Peek();
            state.OnCustomAuthenticationFailed(debugMessage);
        }
    }
}