using UnityEngine;

namespace N.Package.GameSystems
{
    public abstract class GameSystem : MonoBehaviour
    {
        /// <summary>
        /// Initialize the state of this system from the current scene.
        /// If we have any persistent state, reset it.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Shutdown this system.
        /// </summary>
        public abstract void Dispose();
    }
}