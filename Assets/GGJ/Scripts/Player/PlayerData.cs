using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData", fileName = "PlayerData.asset")]
public class PlayerData : ScriptableObject
{
    [Range(0.1f, 50.0f)] public float m_playerSpeed = 5.0f;
}
