using System.Collections.Generic;
using System.Linq;
using N.Package.Bind;
using N.Package.GameSystems.Infrastructure;
using N.Package.Optional;
using UnityEngine;

namespace N.Package.GameSystems
{
  public abstract class SystemsManager : MonoBehaviour
  {
    public List<GameSystemReference> Systems;

    private Registry _services;

    private AtInterval _updateSystemsAtInterval;

    private void Awake()
    {
      _updateSystemsAtInterval = new AtInterval()
      {
        Interval = 0.1f,
        Action = SyncEnabledSystems
      };

      SyncEnabledSystems();
    }

    private void SyncEnabledSystems()
    {
      Systems.ForEach(i => i.Exists = i.Tracker.Update(i.Enabled, i.Template));
    }

    public void Update()
    {
      _updateSystemsAtInterval.Update(Time.deltaTime);
    }

    protected abstract IServiceModule Services();

    public Option<T> System<T>() where T : GameSystem
    {
      var systemRef = Systems.FirstOrDefault(i => i.Exists && i.Tracker.Instance.GetType() == typeof(T));
      return systemRef == null ? Option.None<T>() : Option.Some(systemRef.Tracker.Instance as T);
    }

    /// <summary>
    /// Return the registry to resolve service dependencies.
    /// </summary>
    public Registry Registry
    {
      get
      {
        if (_services != null) return _services;
        _services = new Registry();
        _services.Register(Services());
        return _services;
      }
    }
  }
}