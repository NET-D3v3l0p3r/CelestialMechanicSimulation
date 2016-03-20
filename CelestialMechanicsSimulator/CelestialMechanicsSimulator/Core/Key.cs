using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CelestialMechanicsSimulator.Core
{
    public class Key
    {
        public Keys LocalKey { get; set; }
        public bool IsLocked { get; set; }
        public bool EnableLock { get; set; }

        public Key(Keys key, bool enable_lock)
        {
            LocalKey = key;
            IsLocked = false;
            EnableLock = enable_lock;
        }

        public override string ToString()
        {
            return LocalKey + "";
        }
    }
}
