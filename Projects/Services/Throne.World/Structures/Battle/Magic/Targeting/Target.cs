using System;
using Throne.World.Network;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Travel;

namespace Throne.World.Structures.Battle.Targeting
{
    public class Target : IWorldObject
    {
        public readonly WorldObject Object;

        public Int32 EffectValue;
        public Magic.ResponseEffect Effects;
        public Int32 Value;


        public Target(WorldObject obj)
        {
            Object = obj;
        }

        public uint ID
        {
            get { return Object.ID; }
        }

        public Location Location
        {
            get { return Object.Location; }
            set { throw new InvalidOperationException(); }
        }

        public void SpawnFor(WorldClient observer)
        {
            throw new InvalidOperationException();
        }

        public void SetEffects(Magic.ResponseEffect effects, Int32 eVal)
        {
            Effects = effects;
            EffectValue = eVal;
        }

        public void SetValue(Int32 val)
        {
            Value = val;
        }

        public static implicit operator WorldObject(Target tgt)
        {
            return tgt.Object;
        }

        public static implicit operator Boolean(Target tgt)
        {
            return tgt.Object.Attackable();
        }

        public override string ToString()
        {
            return Object.ToString();
        }
    }
}