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
        private FlowStateBase m_nextState;

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

        public void ChangeState(FlowStateBase state)
        {
            m_nextState = state;
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
                    if (m_nextState != null)
                    {
                        PushState(m_nextState);
                        m_nextState = null;
                    }
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
        
        public void OnConnected()
        {
            var state = m_stateStack.Peek();
            state.OnConnected();
        }

        public void OnConnectedToMaster()
        {
            var state = m_stateStack.Peek();
            state.OnConnectedToMaster();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            var state = m_stateStack.Peek();
            state.OnDisconnected(cause);
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            var state = m_stateStack.Peek();
            state.OnRegionListReceived(regionHandler);
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            var state = m_stateStack.Peek();
            state.OnCustomAuthenticationResponse(data);
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            var state = m_stateStack.Peek();
            state.OnCustomAuthenticationFailed(debugMessage);
        }
        
        public void OnCreatedRoom()
        {
            var state = m_stateStack.Peek();
            state.OnCreatedRoom();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            var state = m_stateStack.Peek();
            state.OnCreateRoomFailed(returnCode,message);
        }

        public void OnJoinedRoom()
        {
            var state = m_stateStack.Peek();
            state.OnJoinedRoom();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            var state = m_stateStack.Peek();
            state.OnJoinRandomFailed(returnCode,message);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            var state = m_stateStack.Peek();
            state.OnJoinRandomFailed(returnCode,message);
        }

        public void OnLeftRoom()
        {
            var state = m_stateStack.Peek();
            state.OnLeftRoom();
        }
        
        public void OnJoinedLobby()
        {
            var state = m_stateStack.Peek();
            state.OnJoinedLobby();
        }

        public void OnLeftLobby()
        {
            var state = m_stateStack.Peek();
            state.OnLeftLobby();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            var state = m_stateStack.Peek();
            state.OnRoomListUpdate(roomList);
        }
    }
}