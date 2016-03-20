using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialMechanicsSimulator.Core
{
    public class InputManager
    {
        public Dictionary<Key, KeyAction> KeyAssignments { get; private set; }

        public bool KeyDown { get; private set; }

        public InputManager()
        {
            KeyAssignments = new Dictionary<Key, KeyAction>();
        }

        public void BindKey(Key key, KeyAction action)
        {
            if (!KeyAssignments.Keys.Contains(key))
            {
                KeyAssignments.Add(key, action);
            }
            else Debug.WriteLine("Key: " + key + " is already used!");
        }

        public void Update()
        {
            for(int i = 0; i < KeyAssignments.Count; i++)
            {
                switch (KeyAssignments.Keys.ElementAt(i).EnableLock)
                {
                    case true:
                        if (Keyboard.GetState().IsKeyDown(KeyAssignments.Keys.ElementAt(i).LocalKey) && !KeyAssignments.Keys.ElementAt(i).IsLocked)
                        {
                            KeyAssignments[KeyAssignments.Keys.ElementAt(i)].RaisePress();
                            KeyAssignments.Keys.ElementAt(i).IsLocked = true;
                        }
                        else if (Keyboard.GetState().IsKeyUp(KeyAssignments.Keys.ElementAt(i).LocalKey) && KeyAssignments.Keys.ElementAt(i).IsLocked)
                        {
                            KeyAssignments[KeyAssignments.Keys.ElementAt(i)].RaiseUp();
                            KeyAssignments.Keys.ElementAt(i).IsLocked = false;
                        }
                        break;
                    case false:
                        if (Keyboard.GetState().IsKeyDown(KeyAssignments.Keys.ElementAt(i).LocalKey))
                        {
                            KeyAssignments[KeyAssignments.Keys.ElementAt(i)].RaisePress();
                            KeyAssignments.Keys.ElementAt(i).IsLocked = true;
                        }
                        if (Keyboard.GetState().IsKeyUp(KeyAssignments.Keys.ElementAt(i).LocalKey) && KeyAssignments.Keys.ElementAt(i).IsLocked) 
                        {
                            KeyAssignments[KeyAssignments.Keys.ElementAt(i)].RaiseUp();
                            KeyAssignments.Keys.ElementAt(i).IsLocked = false;
                        }
                        break;
                }
            }

        }
    }
}
