using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputKeys", fileName = "InputKeys.asset")]
public class InputKeys : ScriptableObject
{
    public KeyCodeSet m_keyCodes = new KeyCodeSet() { m_upKey = KeyCode.W, m_downKey = KeyCode.S, m_leftKey = KeyCode.A, m_rightKey = KeyCode.D, m_runKey = KeyCode.LeftShift, m_crouchToggleKey = KeyCode.LeftControl };
}

[Serializable]
public struct KeyCodeSet
{
    public KeyCode m_upKey;
    public KeyCode m_downKey;
    public KeyCode m_leftKey;
    public KeyCode m_rightKey;
    public KeyCode m_runKey;
    public KeyCode m_crouchToggleKey;
}