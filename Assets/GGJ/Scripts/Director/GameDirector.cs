using PersonalFramework;
using UnityEngine;

public class GameDirector : LocalDirector
{
    [RuntimeInitializeOnLoadMethod]
    private static void StartDirector()
    {
        GameObject director = new GameObject("GameDirector");
        GameDirector gameDirector = director.AddComponent<GameDirector>();
        DontDestroyOnLoad(director);
    }

    private void Start()
    {
        m_stateController.PushState(new BaseGameState());
    }
}