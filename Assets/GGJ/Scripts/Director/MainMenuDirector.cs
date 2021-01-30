using PersonalFramework;

public class MainMenuDirector : LocalDirector
{
    private void Start()
    {
        m_stateController.PushState(new MainMenuState());
    }
}
