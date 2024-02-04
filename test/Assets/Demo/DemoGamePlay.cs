using System.Threading.Tasks;
using Demo.Services;
using N.Package.GameSystems;
using N.Package.GameSystems.Utility.SceneTools;
using N.Package.NSystems.Utility.SceneTools;
using N.Package.Optional;
using N.Package.Promises;
using UnityEngine;

namespace Demo
{
    public class DemoGamePlay : MonoBehaviour
    {
        private readonly LazySystem<PrefabSystem> _prefabs = new LazySystem<PrefabSystem>();

        public ILogService LogService { private get; set; }
        public NSceneToolsService SceneTools { private get; set; }
        public NSceneComponent targetScene;
        public GameObject spawnInNewScene;
        public float openSceneIn = 5;
        private float _remaining;

        public GameObject overrideTemplate;
        private bool _ready;
        private float _elapsed;

        public void Start()
        {
            Systems.Registry.Bind(this);

            var manager = FindObjectOfType<DemoSystemsManager>();
            manager.Initialize<PrefabSystem>((system) => { system.Template1 = overrideTemplate; });
        }

        public void Update()
        {
            if (!_ready)
            {
                Initialize();
            }

            _elapsed += Time.deltaTime;
            if (_elapsed > 1f)
            {
                _elapsed = 0f;
                _remaining -= 1f;
                Debug.Log($"Remaining time to transition: {_remaining}");
                if (_remaining <= 0f)
                {
                    OnOpenSceneViaLinkAsync().Dispatch();
                }
            }
        }

        private void Initialize()
        {
            _ready = true;
            _remaining = openSceneIn;
            _elapsed = 0f;

            _prefabs.Value.With((prefabs) =>
            {
                prefabs.MakeInstance(PrefabSystem.Prefabs.Template1, Vector3.zero);
                prefabs.MakeInstance(PrefabSystem.Prefabs.Template2, Vector3.one);
                LogService.Log("Spawned using game system!");
            });
        }

        public async Task OnOpenSceneViaLinkAsync()
        {
            Debug.Log("Populating scene message queue");
            for (var i = 0; i < 10; i++)
            {
                var stream = new NSceneEventStream<DemoTask>();
                stream.Add(new DemoTask()
                {
                    Message = $"message -- {Random.value} -- {i}"
                });
            }
            
            var template = spawnInNewScene;
            await SceneTools.Open(targetScene);
            Debug.Log("Executing post-scene load tasks on new scene!");
            Instantiate(template);
        }
    }
}