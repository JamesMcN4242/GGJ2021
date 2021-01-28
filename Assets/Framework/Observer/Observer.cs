////////////////////////////////////////////////////////////
/////   Observer.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace PersonalFramework
{
    public class Observer
    {
        private const int k_startingMessageCapacity = 3;
        private List<object> m_messageObjects = new List<object>(k_startingMessageCapacity);

        public virtual void ObserveMessage(object message)
        {
            m_messageObjects.Add(message);
        }

        public object[] ConsumeMessages()
        {
            object[] msgs = m_messageObjects.ToArray();
            m_messageObjects.Clear();
            return msgs;
        }
    }
}