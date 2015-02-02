using System;
using System.Collections.Generic;
using Throne.World.Structures.Objects.Actors;
using Throne.World.Structures.Travel;

namespace Throne.World.Structures.Objects
{
    abstract partial class Role
    {
        protected Dictionary<UInt32, Character> _currentVisibleCharacters;
        protected Dictionary<UInt32, Item> _currentVisibleMapItems;
        protected Dictionary<UInt32, Npc> _currentVisibleNpcs;

        public void ClearScreen()
        {
            _currentVisibleCharacters.Clear();
            _currentVisibleMapItems.Clear();
            _currentVisibleNpcs.Clear();
        }

        public Boolean CanSee(IWorldObject obj)
        {
            return _currentVisibleCharacters.ContainsKey(obj.ID) ||
                   _currentVisibleMapItems.ContainsKey(obj.ID) ||
                   _currentVisibleNpcs.ContainsKey(obj.ID);
        }

        public Boolean GetVisibleObject<TObject>(UInt32 objId, out TObject obj)
            where TObject : WorldObject
        {
            WorldObject result = obj = null;

            try
            {
                result = _currentVisibleCharacters[objId];
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    result = _currentVisibleNpcs[objId];
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        result = _currentVisibleMapItems[objId];
                    }
                    catch (KeyNotFoundException)
                    {
                        return false;
                    }
                }
            }

            return obj = result as TObject;
        }

        public abstract void LookDown (Jump jmp);

        public abstract void LookAround ();
    }
}