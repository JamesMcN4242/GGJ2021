using System;
using Photon.Pun;
using UnityEngine;

namespace GGJ.Scripts
{
    public class Move : MonoBehaviour, IPunObservable
    {
        public float m_speed = 5f;
        
        public void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * m_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * m_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.back * m_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * m_speed * Time.deltaTime;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                Vector3 pos = transform.position;
                stream.Serialize(ref pos);
            }
            else
            {
                Vector3 pos = Vector3.zero;
                stream.Serialize(ref pos);
                transform.position = pos;
            }
        }
    }
}