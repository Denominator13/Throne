namespace Throne.World.Structures.Battle.Targeting.Filters
{
    public abstract class TargetFilter
    {
        public abstract void Invoke(BattleInteraction usage, ref TargetingCollection targets);
    }

    /// <summary>
    ///     Removes any presently invulnerable WorldObject.
    /// </summary>
    public class AttackableFilter : TargetFilter
    {
        public override void Invoke(BattleInteraction usage, ref TargetingCollection targets)
        {
            targets.RemoveAll(t => !t.Attackable());
        }
    }
}