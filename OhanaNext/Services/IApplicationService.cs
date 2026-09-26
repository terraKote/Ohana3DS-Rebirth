using System.Windows;

namespace OhanaNext.Services;

public interface IApplicationService
{
    void Exit();
}

public class ApplicationService : IApplicationService
{
    public void Exit()
    {
        Application.Current.Shutdown();
    }
}