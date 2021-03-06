﻿using System;
using Photon.Pun;
using UnityEngine;

public class PositionMono : MonoBehaviour, IPunObservable
{
    public bool m_escaped = false;
    
    public bool IsSeeker = false;
    private Animator m_animator;
    [HideInInspector] public Vector3 m_velocity;

    private Vector3 m_start;
    private Vector3 m_end;
    private float m_currentTime = 0;
    private float m_lag;

    private Quaternion m_startRot;
    private Quaternion m_endRot;
    
    private PlayerAudioMovement m_audioMovement;

    public void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_audioMovement = GetComponent<PlayerAudioMovement>();
    }

    public void LateUpdate()
    {
        if (Mathf.Abs(m_lag) > 0)
        {
            transform.position = Vector3.Lerp(m_start, m_end, m_currentTime / m_lag);   
            transform.rotation = Quaternion.Slerp(m_startRot,m_endRot,m_currentTime/m_lag);
        }
        m_currentTime += Time.deltaTime;

        m_animator.SetFloat("MovementSpeed", m_velocity.magnitude);
        m_audioMovement.m_walkingSpeed = m_velocity.magnitude;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(m_velocity);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector3 position = (Vector3)stream.ReceiveNext();
            m_velocity = (Vector3) stream.ReceiveNext();
            m_startRot = transform.rotation;
            m_endRot = (Quaternion)stream.ReceiveNext();

            m_start = transform.position;
            m_lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime)) * 2f;
            m_end = position + m_velocity * m_lag;
            m_currentTime = 0;
        }
    }
}
