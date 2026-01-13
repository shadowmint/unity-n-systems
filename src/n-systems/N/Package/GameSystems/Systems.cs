using N.Package.Bind;
using N.Package.Optional;
using UnityEngine;

namespace N.Package.GameSystems
{
  public static class Systems
  {
    public static Registry Registry => Instance.Registry;

    public static Option<T> System<T>() where T : GameSystem
    {
      return Instance == null ? Option.None<T>() : Instance.System<T>();
    }

    private static SystemsManager _instance;

    private static SystemsManager Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = Object.FindFirstObjectByType<SystemsManager>();
        }

        return _instance;
      }
    }
  }
}