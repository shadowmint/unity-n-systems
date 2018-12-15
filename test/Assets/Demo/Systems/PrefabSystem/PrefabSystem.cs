using System;
using N.Package.GameSystems;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo
{
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
}