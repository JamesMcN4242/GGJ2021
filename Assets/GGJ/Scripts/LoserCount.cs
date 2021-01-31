using System;
using UnityEngine;

public class LoserCount : MonoBehaviour
{
    public int m_loserCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        ++m_loserCount; // this is terrible and if people manage to make it to the end of the area and back it will break it
    }
}
