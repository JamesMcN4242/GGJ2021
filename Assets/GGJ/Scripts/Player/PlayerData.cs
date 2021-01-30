using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData", fileName = "PlayerData.asset")]
public class PlayerData : ScriptableObject
{
    [Range(0.1f, 50.0f)] public float m_walkSpeed = 5.0f;
    [Range(0.1f, 50.0f)] public float m_runSpeed = 7.0f;
    [Range(0.1f, 50.0f)] public float m_crouchSpeed = 5.0f;

    public Vector3 m_standingSize = Vector3.one;
    public Vector3 m_crouchSize = Vector3.one;
}
