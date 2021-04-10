using System;
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
        public List<GameSystemReference> systems;

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
            systems.ForEach(i => i.Exists = i.Tracker.Update(i.Enabled, i.Template, i.Exists));
        }

        public void Update()
        {
            _updateSystemsAtInterval.Update(Time.deltaTime);
        }

        /// <summary>
        /// Explicitly initialize a system by catching the instance before the Initialize method is called.
        /// You can use this to override the state of system before it is initialized, eg. for tests.
        /// Note this is a one time action; disabling and enabling the system will skip this.
        /// </summary>
        public void Initialize<T>(Action<T> preInitHook) where T : GameSystem
        {
            systems.Where(i => i.Template.GetType() == typeof(T))
                .ToList()
                .ForEach(system => { system.Tracker.Update(system.Enabled, system.Template, preInitHook, system.Exists); });
        }

        protected abstract IServiceModule Services();

        public Option<T> System<T>() where T : GameSystem
        {
            var systemRef = systems.FirstOrDefault(i => i.Exists && i.Tracker.Instance.GetType() == typeof(T));
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