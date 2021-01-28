////////////////////////////////////////////////////////////
/////   WatchedObject.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

namespace PersonalFramework
{
    public class WatchedObject : MonoBehaviour
    {
        private const int k_startingObserverCapacity = 5;
        private List<Observer> m_observers = new List<Observer>(k_startingObserverCapacity);

        public void AddObserver(Observer observer)
        {
            if(!m_observers.Contains(observer))
            {
                m_observers.Add(observer);
            }
        }

        public void RemoveObserver(Observer observer)
        {
            if(m_observers.Contains(observer))
            {
                m_observers.Remove(observer);
            }
        }

        public void ClearAllObservers()
        {
            m_observers.Clear();
        }

        public void NotifyObservers(object objectToNotify)
        {
            for(int i = 0; i < m_observers.Count; i++)
            {
                m_observers[i]?.ObserveMessage(objectToNotify);
            }
        }
    }
}