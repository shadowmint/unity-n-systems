namespace N.Package.GameSystems.Infrastructure
{
  [System.Serializable]
  public class GameSystemReference
  {
    public bool Enabled;
    public bool Exists;
    public GameSystem Template;
    public readonly GameSystemTracker Tracker = new GameSystemTracker();
  }
}