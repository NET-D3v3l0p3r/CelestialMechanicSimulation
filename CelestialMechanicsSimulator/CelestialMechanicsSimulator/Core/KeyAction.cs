using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CelestialMechanicsSimulator.Core
{
    public class KeyAction
    {
        private Action KeyPress, KeyUp;

        public KeyAction(Action press, Action up)
        {
            KeyPress = press;
            KeyUp = up;
        }

        public void RaisePress()
        {
            KeyPress.Invoke();
        }
        public void RaiseUp()
        {
            KeyUp.Invoke();
        }
    }
}
