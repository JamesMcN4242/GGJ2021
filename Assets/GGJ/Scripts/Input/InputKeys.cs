using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputKeys", fileName = "InputKeys.asset")]
public class InputKeys : ScriptableObject
{
    public KeyCodeSet m_keyCodes;
}

[Serializable]
public struct KeyCodeSet
{
    public KeyCode m_upKeyCode;
    public KeyCode m_downKeyCode;
    public KeyCode m_leftKeyCode;
    public KeyCode m_rightKeyCode;
}