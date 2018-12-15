# unity-n-systems

This is base library for very common patterns for game systems.

## Usage

See the tests in the `Editor/` folder for each class for usage examples.

### tldr

Extend `SystemsManager` with a project specific top level system manager:

    public class DemoSystemsManager : SystemsManager
    {
      protected override IServiceModule Services()
      {
        return new DemoSystemsConfig();
      }
    }

    public class DemoSystemsConfig : IServiceModule
    {
      public void Register(ServiceRegistry registry)
      {
        registry.Register<LogService>();
      }
    }

Implement any arbitrary number of game systems by extending `GameSystem`:

    public class PrefabSystem : GameSystem
    {
      public GameObject Template1;

      public GameObject Template2;

      public GameObject MakeInstance(Prefabs target, Vector3 pos)
      {
        switch (target)
        {
          case Prefabs.Template1:
            return Make(Template1, pos);
          case Prefabs.Template2:
            return Make(Template2, pos);
          default:
            throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
      }

      private GameObject Make(GameObject template, Vector3 pos)
      {
        var instance = Instantiate(template);
        instance.transform.position = pos;
        return instance;
      }

      public override void Initialize()
      {
      }

      public override void Dispose()
      {
      }

      public enum Prefabs
      {
        Template1,
        Template2
      }
    }

Every game system should be converted into a prefab and attached to the global systems
manager instance (look at the demo in `tests` for an example).

Arbitrary components can now consume game systems using `LazySystem` or arbitrary service
bindings using `System.Registry`:

    public class DemoGamePlay : MonoBehaviour
    {
      private readonly LazySystem<PrefabSystem> _prefabs = new LazySystem<PrefabSystem>();

      public ILogService LogService { private get; set; }

      public void Start()
      {
        Systems.Registry.Bind(this);
        _prefabs.Value.With((prefabs) =>
        {
          prefabs.MakeInstance(PrefabSystem.Prefabs.Template1, Vector3.zero);
          prefabs.MakeInstance(PrefabSystem.Prefabs.Template2, Vector3.one);
          LogService.Log("Spawned using game system!");
        });
      }
    }

## Install

From your unity project folder:

    npm init
    npm install shadowmint/unity-n-systems --save
    echo Assets/pkg-all >> .gitignore
    echo Assets/pkg-all.meta >> .gitignore

The package and all its dependencies will be installed in
your Assets/pkg-all folder.

## Development

Setup and run tests:

    npm install
    npm install ..
    cd test
    npm install

### Tests

Tests are located in the `tests` folder.
