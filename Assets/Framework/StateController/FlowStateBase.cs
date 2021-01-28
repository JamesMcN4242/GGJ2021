////////////////////////////////////////////////////////////
/////   FlowStateBase.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

namespace PersonalFramework
{
    /// <summary>
    /// State base for controlling logical states
    /// </summary>
    public abstract class FlowStateBase
    {
        private enum Status { START_PRESENTING, PRESENTING, START_ACTIVE, ACTIVE, START_DISMISSING, DISMISSING, DISMISSED };
        private Status m_status = Status.START_PRESENTING;
        protected UIStateBase m_ui = null;
        private Observer m_messageObserver = new Observer();
        
        protected StateController ControllingStateStack { get; private set; }

        public void SetStateController(StateController stateController)
        {
            ControllingStateStack = stateController;
        }

        public void UpdateState()
        {
            switch (m_status)
            {
                case Status.START_PRESENTING:
                    BeginPresentingState();
                    break;

                case Status.PRESENTING:
                    UpdatePresentingState();
                    break;

                case Status.START_ACTIVE:
                    BeginActiveState();
                    break;

                case Status.ACTIVE:
                    UpdateActiveState();
                    break;

                case Status.START_DISMISSING:
                    BeginDismissingState();
                    break;

                case Status.DISMISSING:
                    UpdateDismissingState();
                    break;
            }

            object[] messages = m_messageObserver.ConsumeMessages();
            for(int i = 0; i < messages.Length; i++)
            {
                HandleMessage(messages[i]);
            }
        }

        public void FixedUpdateState()
        {
            switch (m_status)
            {
                case Status.PRESENTING:
                    FixedUpdatePresenting();
                    break;

                case Status.ACTIVE:
                    FixedUpdateActiveState();
                    break;

                case Status.DISMISSING:
                    FixedDismissingState();
                    break;
            }
        }

        protected virtual void HandleMessage(object message)
        {
        }

        protected virtual bool AquireUIFromScene()
        {
            return false;
        }

        /// <summary>
        /// When the state starts it's presenting state
        /// </summary>
        private void BeginPresentingState()
        {
            if (AquireUIFromScene())
            {
                m_ui.SetContentActiveStatus(true);
                RebuildObserverList();
            }

            StartPresentingState();
            m_status = Status.PRESENTING;
        }

        protected virtual void StartPresentingState()
        {
        }

        /// <summary>
        /// Update the presenting state of the state
        /// </summary>
        protected virtual void UpdatePresentingState()
        {
            EndPresentingState();
        }

        /// <summary>
        /// When the state enters it's active state
        /// </summary>
        private void BeginActiveState()
        {
            StartActiveState();
            m_status = Status.ACTIVE;
        }

        protected virtual void StartActiveState()
        {
        }

        /// <summary>
        /// Update the active state
        /// </summary>
        protected virtual void UpdateActiveState()
        {
        }

        private void BeginDismissingState()
        {
            StartDismissingState();
            m_status = Status.DISMISSING;
        }

        protected virtual void StartDismissingState()
        {
        }

        protected virtual void UpdateDismissingState()
        {
            EndDismissingState();
        }

        protected void EndPresentingState()
        {
            m_status = Status.START_ACTIVE;
        }

        /// <summary>
        /// When the state ends it's active state
        /// </summary>
        public void EndActiveState()
        {
            m_status = Status.START_DISMISSING;
        }

        public void EndDismissingState()
        {
            m_status = Status.DISMISSED;
            if (m_ui != null)
            {
                m_ui.SetContentActiveStatus(false);

                WatchedObject[] objs = m_ui.gameObject.GetComponentsInChildren<WatchedObject>(true);
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].RemoveObserver(m_messageObserver);
                }
            }
        }

        public bool IsDismissed()
        {
            return m_status == Status.DISMISSED;
        }

        protected void RebuildObserverList()
        {
            if (m_ui != null)
            {
                WatchedObject[] objs = m_ui.gameObject.GetComponentsInChildren<WatchedObject>(true);
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].AddObserver(m_messageObserver);
                }
            }
        }

        protected virtual void FixedUpdatePresenting()
        {
        }

        protected virtual void FixedUpdateActiveState()
        {
        }

        protected virtual void FixedDismissingState()
        {
        }
    }
}