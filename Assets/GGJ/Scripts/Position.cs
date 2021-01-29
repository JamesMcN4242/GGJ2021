using Photon.Pun;
using UnityEngine;

namespace GGJ.Scripts
{
    public class Position : MonoBehaviour, IPunObservable
    {
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