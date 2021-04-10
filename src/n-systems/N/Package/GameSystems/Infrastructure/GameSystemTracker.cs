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

        public bool Update(bool systemState, GameSystem prefab, bool exists)
        {
            return Update<GameSystem>(systemState, prefab, null, exists);
        }

        public bool Update<T>(bool systemState, GameSystem prefab, Action<T> preInitHook, bool exists) where T : class
        {
            // If the state is 'exists' but no concrete instance currently exists, try to find it.
            if (exists && systemState && !_currentState)
            {
                var concreteType = prefab.GetComponent<GameSystem>().GetType();
                Debug.Log(concreteType);

                var instance = Object.FindObjectOfType(concreteType) as GameSystem;
                if (instance != null)
                {
                    _instance = instance;
                    _currentState = true;
                }
            }

            // Process as normal
            if (_currentState == systemState) return _currentState;
            if (systemState)
            {
                SpawnAndInitializeSystem(prefab, preInitHook);
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

        private void SpawnAndInitializeSystem<T>(GameSystem prefab, Action<T> preInitHook) where T : class
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

            if (preInitHook != null)
            {
                try
                {
                    preInitHook(_instance as T);
                }
                catch (Exception error)
                {
                    Debug.LogError($"Failed to run preInitHook on game system: {_instance}: {error}");
                }
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