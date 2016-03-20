using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace CelestialMechanicsSimulator.Core.Observer
{
    public class Observer
    {
        public Solarsystem Handler { get; private set; }
        public GraphicsDevice Device { get; private set; }
        public Camera Camera { get; set; }
        public InputManager InputManager { get; private set; }

        public string Name { get; private set; }

        public Observer(Solarsystem handler, GraphicsDevice device, string name)
        {
            Handler = handler;
            Device = device;
            Camera = new Camera(Handler, Device, new Vector3(0, 3945000, 0), .0008f, 9000.0f);
            Name = name;
            
            InputManager = new InputManager();
            InputManager.BindKey(new Key(Keys.W, false), new KeyAction(new Action(() =>
            {
                Camera.Move(new Vector3(0, 0, -1));
            }), new Action(() => {  })));
            InputManager.BindKey(new Key(Keys.A, false), new KeyAction(new Action(() =>
            {
                Camera.Move(new Vector3(-1, 0, 0));
            }), new Action(() => {  })));
            InputManager.BindKey(new Key(Keys.S, false), new KeyAction(new Action(() =>
            {
                Camera.Move(new Vector3(0, 0, 1));
            }), new Action(() => {  })));
            InputManager.BindKey(new Key(Keys.D, false), new KeyAction(new Action(() =>
            {
                Camera.Move(new Vector3(1, 0, 0));
            }), new Action(() => { })));
            Handler.SetInputManager(InputManager);
        }
    }
}
