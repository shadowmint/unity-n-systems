using System.Collections;
using System.Threading.Tasks;
using N.Package.NSystems.Utility.SceneTools;
using N.Package.Promises;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace N.Package.GameSystems.Utility.SceneTools
{
    public class NSceneToolsService
    {
        public Task Open(NSceneComponent sceneRef)
        {
            return Open(sceneRef.targetScene);
        }
        
        public Task Open(string scenePath)
        {
            LazySystem.ResetAll();

            var waiter = new TaskCompletionSource<bool>();

            IEnumerator WaitForLoad()
            {
                var asyncLoad = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                yield return new WaitForEndOfFrame();
                waiter.SetResult(true);
            }

            AsyncWorker.RunAsync(WaitForLoad);
            return waiter.Task;
        }
    }
}