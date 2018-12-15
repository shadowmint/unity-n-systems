using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace N.Package.GameSystems.Infrastructure
{
  public class GameSystemTracker
  {
    private bool _currentState;

    private GameSystem _instance;

    public GameSystem Instance => _instance;

    public bool Update(bool systemState, GameSystem prefab)
    {
      if (_currentState == systemState) return _currentState;
      if (systemState)
      {
        SpawnAndInitializeSystem(prefab);
      }
      else
      {
        ShutdownAndDestroySystem();
      }

      _currentState = systemState;
      return _currentState;
    }

    private void ShutdownAndDestroySystem()
    {
      if (_instance == null) return;
      try
      {
        _instance.Dispose();
      }
      catch (Exception error)
      {
        Debug.LogError($"Failed to shutdown game system: {_instance}: {error}");
      }

      try
      {
        Object.Destroy(_instance.gameObject);
      }
      catch (Exception error)
      {
        Debug.LogError($"Failed to destroy game system: {_instance}: {error}");
        _instance = null;
      }
    }

    private void SpawnAndInitializeSystem(GameSystem prefab)
    {
      try
      {
        ShutdownAndDestroySystem();
        _instance = Object.Instantiate(prefab);
        _instance.transform.name = _instance.GetType().Name;
      }
      catch (Exception error)
      {
        Debug.LogError($"Failed to create game system: {_instance}: {error}");
        _instance = null;
        return;
      }

      try
      {
        _instance.Initialize();
      }
      catch (Exception error)
      {
        Debug.LogError($"Failed to create game system: {_instance}: {error}");
        ShutdownAndDestroySystem();
        return;
      }

      Debug.Log($"Initialised game system: {_instance}");
    }
  }
}