using System;
using System.Threading.Tasks;
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

        public bool With(Action<T> action)
        {
            return Value.With<T>(action);
        }

        public Task<bool> WithAsync(Func<T, Task> action)
        {
            return Value.WithAsync(action);
        }

        public TRtn WithValue<TRtn>(Func<T, TRtn> action, TRtn defaultValue)
        {
            return Value.WithValue(action, defaultValue);
        }

        public Task<TRtn> WithValueAsync<TRtn>(Func<T, Task<TRtn>> action, TRtn defaultValue)
        {
            return Value.WithValueAsync(action, defaultValue);
        }
    }
}