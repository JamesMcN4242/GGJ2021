using System;
using UnityEngine;

public class BallBounceAmplifier : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        m_rigidbody.AddForce(Vector3.up * 2.5f);
    }
}
