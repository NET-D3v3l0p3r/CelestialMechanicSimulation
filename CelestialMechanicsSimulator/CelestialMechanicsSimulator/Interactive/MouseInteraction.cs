using CelestialMechanicsSimulator.Core;
using CelestialMechanicsSimulator.Core.CelestialBodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialMechanicsSimulator.Interactive
{
    public class MouseInteraction
    {
        //PRIVATE DECLARATIONS
        private int ScrollBuffer;
        //PUBLIC DECLARATIONS
        public Action LeftMouse_Down { get; set; }
        public Action LeftMouse_Up { get; set; }

        public Action MiddleMouse_Down { get; set; }
        public Action MiddleMouse_Up { get; set; }

        public Action RightMouse_Down { get; set; }
        public Action RightMouse_Up { get; set; }

        public Action Scrolling_Up { get; set; }
        public bool IsScrollingUp { get; private set; }

        public Action Scrolling_Down { get; set; }
        public bool IsScrollingDown { get; private set; }

        public bool IsLeftButtonDown { get; private set; }
        public bool IsRightButtonDown { get; private set; }
        public bool IsMiddleButtonDown { get; private set; }

        public void Update()
        {
            MouseState state = Mouse.GetState();

            //if (state.LeftButton == ButtonState.Pressed)
            //    LeftMouse_Down.Invoke();
            //if (state.LeftButton == ButtonState.Released)
            //    LeftMouse_Up.Invoke();

            //if (state.LeftButton == ButtonState.Pressed)
            //    MiddleMouse_Down.Invoke();
            //if (state.LeftButton == ButtonState.Released)
            //    MiddleMouse_Up.Invoke();

            //if (state.LeftButton == ButtonState.Pressed)
            //    RightMouse_Down.Invoke();
            //if (state.LeftButton == ButtonState.Released)
            //    RightMouse_Up.Invoke();

            IsScrollingUp = false;
            IsScrollingDown = false;

            if (state.ScrollWheelValue > ScrollBuffer)
            {
                IsScrollingUp = true;
                IsScrollingDown = false;
                //Scrolling_Up.Invoke();

            }
            if (state.ScrollWheelValue < ScrollBuffer)
            {
                IsScrollingUp = false;
                IsScrollingDown = true;
                //Scrolling_Down.Invoke();
            }

            ScrollBuffer = state.ScrollWheelValue;

        }

        public Vector3 Get3DCoordinates(GraphicsDevice device, Matrix projection, Matrix view)
        {
            return device.Viewport.Unproject(new Vector3(Mouse.GetState().X,
                    Mouse.GetState().Y, 0.0f),
                    projection,
                    view,
                    Matrix.Identity);
        }
        public Ray GetRay(GraphicsDevice device, Matrix projection, Matrix view)
        {
            return ToolKit.CalculateRay(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), view, projection, device.Viewport);
        }
        public Vector3 GetXZAtY(Ray ray, float y)
        {
            // y = mx+b
            // 1 = mx+b
            // 1-b = mx
            // 1-b/m = x
            var lambda = (y - ray.Position.Y) / ray.Direction.Y;

            var xCoord = (double)(ray.Direction.X * lambda + ray.Position.X);
            var yCoord = (double)(ray.Direction.Y * lambda + ray.Position.Y);
            var zCoord = (double)(ray.Direction.Z * lambda + ray.Position.Z);

            return new Vector3((float)xCoord, (float)yCoord, (float)zCoord);
        }
    }
}
