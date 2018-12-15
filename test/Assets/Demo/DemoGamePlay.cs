using Demo.Services;
using N.Package.GameSystems;
using N.Package.Optional;
using UnityEngine;

namespace Demo
{
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
}