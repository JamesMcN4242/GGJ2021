using System;
using UnityEngine;

public class FaceCameraMono : MonoBehaviour
{
    private Camera m_camera;
    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = -(m_camera.transform.position - transform.position).CopyWithY(0).normalized;
    }
}
