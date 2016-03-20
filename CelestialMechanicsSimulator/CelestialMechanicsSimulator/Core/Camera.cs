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
namespace CelestialMechanicsSimulator.Core
{
    public class Camera
    {
        public static bool CameraMoving;

        private bool IsMoving;
        private float oldMouseX, oldMouseY;

        public Solarsystem Handler { get; private set; }
        public GraphicsDevice Device { get; set; }

        public Matrix ProjectionsMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }

        public Vector3 CameraPosition { get; set; }
        public Vector3 Direction { get; private set; }
        public BoundingFrustum ViewFrustum { get; private set; }

        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public float MouseSensity { get; set; }
        public float Velocity { get; set; }

        public Vector3 REFERENCEVECTOR = new Vector3(0, 0, -1);

        public Camera(Solarsystem handler, GraphicsDevice device_g, Vector3 spawn_vec3, float mouse_sensity, float velocity)
        {
            Handler = handler;
            Device = device_g;
            CameraPosition = -spawn_vec3;
            MouseSensity = mouse_sensity;
            Velocity = velocity;
            ProjectionsMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Device.Viewport.AspectRatio, 0.01f, 1500000.0f);
            Flush();
        }

        public void Update()
        {
            if (Handler.IsGameContinuing)
            {
                float dX = Mouse.GetState().X - oldMouseX;
                float dY = Mouse.GetState().Y - oldMouseY;

                Pitch += -MouseSensity * dY;
                Yaw += -MouseSensity * dX;

                Pitch = MathHelper.Clamp(Pitch, -1.5f, 1.5f);

                UpdateMatrices();
                ResetCursor();

                ViewFrustum = new BoundingFrustum(ViewMatrix * ProjectionsMatrix);
            }
            if (IsMoving)
            {
                CameraMoving = true;
                IsMoving = false;
            }
            else CameraMoving = false;
        }


        public void SetPosition(Vector3 coord)
        {
            if (coord.X == CameraPosition.X && coord.Y != CameraPosition.Y && coord.Z != CameraPosition.Z)
                CameraPosition = new Vector3(coord.X, -coord.Y, -coord.Z);
            if (coord.X != CameraPosition.X && coord.Y == CameraPosition.Y && coord.Z != CameraPosition.Z)
                CameraPosition = new Vector3(-coord.X, coord.Y, -coord.Z);
            if (coord.X != CameraPosition.X && coord.Y != CameraPosition.Y && coord.Z == CameraPosition.Z)
                CameraPosition = new Vector3(-coord.X, -coord.Y, coord.Z);
            if (coord.X != CameraPosition.X && coord.Y != CameraPosition.Y && coord.Z != CameraPosition.Z)
                CameraPosition = new Vector3(-coord.X, -coord.Y, -coord.Z);

            Flush();
        }

        public void Move(Vector3 to)
        {
            Vector3 velocity = Vector3.Transform(to, Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw));
            velocity *= Velocity;
            CameraPosition -= velocity;
            Flush();
        }

        public void SetLookAt(Vector3 look_at, float zoom)
        {
            if (look_at.X == CameraPosition.X && look_at.Y != CameraPosition.Y && look_at.Z != CameraPosition.Z)
                CameraPosition = new Vector3(look_at.X, -look_at.Y, -look_at.Z);
            if (look_at.X != CameraPosition.X && look_at.Y == CameraPosition.Y && look_at.Z != CameraPosition.Z)
                CameraPosition = new Vector3(-look_at.X, look_at.Y, -look_at.Z);
            if (look_at.X != CameraPosition.X && look_at.Y != CameraPosition.Y && look_at.Z == CameraPosition.Z)
                CameraPosition = new Vector3(-look_at.X, -look_at.Y, look_at.Z);
            if (look_at.X != CameraPosition.X && look_at.Y != CameraPosition.Y && look_at.Z != CameraPosition.Z)
                CameraPosition = new Vector3(-look_at.X, -look_at.Y, -look_at.Z);

            Vector3 transformation = Vector3.Transform(new Vector3(0, 0, zoom), Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)); ;
            transformation *= Velocity;
            CameraPosition -= transformation;
           
            Flush();

        }

        public void Flush()
        {
            IsMoving = true;
        }

        private void ResetCursor()
        {
            Mouse.SetPosition(Handler.GraphicsDeviceManager.PreferredBackBufferWidth / 2, Handler.GraphicsDeviceManager.PreferredBackBufferHeight / 2);
            oldMouseX = Handler.GraphicsDeviceManager.PreferredBackBufferWidth / 2.0f;
            oldMouseY = Handler.GraphicsDeviceManager.PreferredBackBufferHeight / 2.0f;
        }

        private void UpdateMatrices()
        {
            Matrix rotation = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
            Vector3 transformedVec = Vector3.Transform(REFERENCEVECTOR, rotation);
            Direction = Vector3.Zero + transformedVec;
            ViewMatrix = Matrix.CreateLookAt(Vector3.Zero, Direction, Vector3.Up);
        }

        public void RenderDebug(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Celestial Mechnaic Simulation" + Environment.NewLine +
                "Camera_Position_actual(THEORETICALLY): " + -CameraPosition + Environment.NewLine +
                "Current_VEC::3_Direction: " + Direction + Environment.NewLine +
                "Current_PITCH: " + Pitch + Environment.NewLine +
                "Current_YAW: " + Yaw + Environment.NewLine +
                "SCROLL_STATE: " + (Handler.MouseInteraction.IsScrollingUp ? "UP" : Handler.MouseInteraction.IsScrollingDown ? "DOWN" : "Null") + Environment.NewLine +
                "MOUSE_LOCATION" + (Handler.MouseInteraction.Get3DCoordinates(Device, Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Device.Viewport.AspectRatio, 1f, 1000.0f), Matrix.CreateLookAt(-CameraPosition, Direction, Vector3.Up))) + Environment.NewLine +
                "MOUSE_LOCATION_XZ_AT_Y: " + Handler.MouseInteraction.GetXZAtY(Handler.MouseInteraction.GetRay(Device,
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Device.Viewport.AspectRatio, 1f, 1000.0f), 
                ViewMatrix), 
                CameraPosition.Y), new Vector2(0, 15), Color.DarkGray);
        }
    }
}