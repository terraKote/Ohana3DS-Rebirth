using System.Windows;

namespace OhanaNext.Services;

public interface IApplicationService
{
    void Exit();
    void ShowMessageBox(string message);
}

public class ApplicationService : IApplicationService
{
    public void Exit()
    {
        Application.Current.Shutdown();
    }

    public void ShowMessageBox(string message)
    {
        MessageBox.Show(message, AppConstants.AppName);
    }
}