using UnityEngine;

public static class InputKeyManagement
{
    private const string k_inputSaveKey = "InputKeyCodes";

    public static KeyCodeSet GetSavedOrDefaultKeyCodes()
    {
        string json = PlayerPrefs.GetString(k_inputSaveKey, string.Empty);
        return string.IsNullOrEmpty(json) ? GetDefaultKeys() : JsonUtility.FromJson<KeyCodeSet>(json);
    }

    public static void SaveKeyCodes(KeyCodeSet keySet)
    {
        string json = JsonUtility.ToJson(keySet);
        PlayerPrefs.SetString(k_inputSaveKey, json);
        PlayerPrefs.Save();
    }

    public static void ResetKeyCodes()
    {
        PlayerPrefs.DeleteKey(k_inputSaveKey);
        PlayerPrefs.Save();
    }

    private static KeyCodeSet GetDefaultKeys()
    {
        return Resources.Load<InputKeys>("InputKeys").m_keyCodes;
    }
}
