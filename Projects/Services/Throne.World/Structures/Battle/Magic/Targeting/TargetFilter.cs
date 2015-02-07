namespace Throne.World.Structures.Battle.Targeting.Filters
{
    public abstract class TargetFilter
    {
        public abstract void Invoke(Magic magic);
    }

    /// <summary>
    ///     Removes any presently invulnerable WorldObject.
    /// </summary>
    public class AttackableFilter : TargetFilter
    {
        public override void Invoke(Magic magic)
        {
            magic.Targets.RemoveAll(t => !t);
        }
    }

    /// <summary>
    ///     Removes any non-player targets.
    /// </summary>
    public class NonPlayerFilter : TargetFilter
    {
        public override void Invoke(Magic magic)
        {
            magic.Targets.RemoveAll(t => !t.Object.IsPlayer);
        }
    }
}