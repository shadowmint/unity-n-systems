using N.Package.Optional;

namespace N.Package.GameSystems
{
  /// <summary>
  /// Helper to lazy load services for consumers with no constructor (eg. MonoBehaviour).
  /// </summary>
  public struct LazySystem<T> where T : GameSystem
  {
    private bool _initialized;

    private Option<T> _instance;

    public Option<T> Value
    {
      get
      {
        if (_initialized)
        {
          return _instance;
        }

        _instance = Systems.System<T>();
        _initialized = true;
        return _instance;
      }
    }

    public void Reset()
    {
      _initialized = false;
    }
  }
}