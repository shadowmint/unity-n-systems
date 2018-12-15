namespace Demo.Services.Concrete
{
  public class LogService : ILogService
  {
    public void Log(string message)
    {
      UnityEngine.Debug.Log(message);
    }
  }
}