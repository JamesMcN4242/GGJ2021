using Photon.Pun;
using UnityEngine;

public class PositionMono : MonoBehaviour, IPunObservable
{
    public Vector3 m_velocity;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(m_velocity);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            m_velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            transform.position += m_velocity * lag;
        }
    }
}
