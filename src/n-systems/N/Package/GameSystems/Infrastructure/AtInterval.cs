using System;

namespace N.Package.GameSystems.Infrastructure
{
  public class AtInterval
  {
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// How often to run action.
    /// </summary>
    public float Interval { get; set; } = 1f;

    /// <summary>
    /// How long since the last action
    /// </summary>
    public float Elapsed { get; set; }

    /// <summary>
    /// What action to do.
    /// </summary>
    public Action Action { get; set; }

    public void Update(float delta)
    {
      if (!Enabled) return;

      Elapsed += delta;
      if (!(Elapsed > Interval)) return;
      
      Elapsed = 0f;
      Action();
    }
  }
}