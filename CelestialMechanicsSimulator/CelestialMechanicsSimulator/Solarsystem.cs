using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using CelestialMechanicsSimulator.Core;
using System.Diagnostics;
using CelestialMechanicsSimulator.Core.Observer;
using CelestialMechanicsSimulator.Core.Observer.miscellaneous.Weapon;
using CelestialMechanicsSimulator.Core.CelestialBodies;
using LineBatchTester;
using BloomPostprocess;
using CelestialMechanicsSimulator.Core.Maths;
using System.Threading;
using CelestialMechanicsSimulator.GUI;
using CelestialMechanicsSimulator.Network;
using CelestialMechanicsSimulator.Interactive;

namespace CelestialMechanicsSimulator
{
    public class Solarsystem : Microsoft.Xna.Framework.Game
    {
        public enum SIMULATOR_MODE
        {
            SIMULATOR_SOLARSYSTEM,
            SIMULATOR_THREE_BODY_P
        }

        public static SIMULATOR_MODE SIMULATOR_TYPE = SIMULATOR_MODE.SIMULATOR_SOLARSYSTEM;

        //CORE
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        
        public InputManager InputManager { get; private set; }
        public MouseInteraction MouseInteraction { get; private set; }

        public UI UI { get; private set; }

        public bool IsGameContinuing { get; set; }
        public bool IsExit { get; private set; }
        public List<CelestialBody> CelestialBodies { get; private set; }
        
        //CORE
        private SpriteBatch spriteBatch;
        public Observer Observer { get; private set; }
        private BloomComponent BloomComponent;
        private SpriteFont DebugFont;

        //FPS
        private int _total_frames, fps;
        private float _elapsed_time;

        //MOUSE
        private bool LeftButtonPressed;
        private bool RightButtonPressed;

        private double ClickTimer;
        private int TimerDelay = 250;
        
        //MISCELLANEOUS
        public SpeechRecognizer SpeechRecognizer { get; private set; }
        private NetworkAPI NetworkAPI;
        private Grid3D Grid;
        private bool SunCollisionActive;

        private Vector3 MousePositionSpace;
        private float Zoom = 150;

        private bool ShowDebug;

        public Solarsystem()
        {
            
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            BloomComponent = new BloomPostprocess.BloomComponent(this);
            Components.Add(BloomComponent);
            CelestialBodies = new List<CelestialBody>();

           
        }
        protected override void Initialize()
        {
            base.Initialize();
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Location = new System.Drawing.Point(0, -12);
            form.TopMost = false;
        }
        protected override void LoadContent()
        {
            #region "CONSOLE"
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(".::XNA CELESTIAL MECHANICS SIMULATION::.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("PROGRAMMED;BIIZNILLAH; BY φConst");
            Console.WriteLine();
            Console.WriteLine("CREDITS: ");
            Console.WriteLine("THE SUN EFFECT ACHIEVED WITH THE BLOOMCOMPONENT.CS AND EFFECT FILES OF http://xbox.create.msdn.com/en-US/education/catalog/sample/bloom");
            Console.WriteLine("THE FORMULAS; PLOTTERS; METHODS;");
            Console.WriteLine("USED IN THIS APPLICATION ARE LISTED IN THE REGISTER");
            Console.WriteLine("THIS APPLICATION USES A FIXED POINT METHOD!!!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ATTENTION: THE CALCULATED VALUES IN THIS APPLICATION ARE APPROXIMATIONS!!!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("SPECS: " + GraphicsDevice.DisplayMode);
            System.Threading.Thread.Sleep(1000);
            //Console.Write("VSYNCH ON/OFF: ");
            //bool vsync = Console.ReadLine().ToUpper() == "ON";
            Console.Write("SIMULATOR_MODE SOLAR/TBODY: ");
            SIMULATOR_TYPE = Console.ReadLine().ToUpper() == "SOLAR" ? SIMULATOR_MODE.SIMULATOR_SOLARSYSTEM : SIMULATOR_MODE.SIMULATOR_THREE_BODY_P;
            Console.WriteLine("Listening to commands...");
            new Thread(new ThreadStart(() =>
            {
                while (!IsExit)
                {
                    Console.Write("root>");
                    string data = Console.ReadLine();
                    switch (data.ToUpper())
                    {
                        case "EXIT":
                            Exit();
                            Environment.Exit(Environment.ExitCode);
                            break;
                        case "RESTART":
                            Exit();
                            System.Windows.Forms.Application.Restart();
                            Environment.Exit(Environment.ExitCode);
                            break;

                    }
                }
            })).Start();
            #endregion
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Loading content...");
            Console.ForegroundColor = ConsoleColor.Gray;
            this.IsFixedTimeStep = false;
            this.GraphicsDeviceManager.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            this.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            this.GraphicsDeviceManager.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            InputManager = new InputManager();
            DebugFont = Content.Load<SpriteFont>("DebugFont");

            LineBatch.Init(GraphicsDevice);
            Observer = new Observer(this, GraphicsDevice, "Observer1");
            CelestialBody.GLOBALSCALE = 50000.0f;
            CelestialBody.DEBUGFONT = DebugFont;
            LoadCelestialBodies();
            IsGameContinuing = true;

            InputManager.BindKey(new Key(Keys.Y, true), new KeyAction(new Action(() =>
            {
                IsGameContinuing = !IsGameContinuing;
                IsMouseVisible = !IsMouseVisible;
                if (!IsMouseVisible)
                    Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);


            }), new Action(() => { })));

            InputManager.BindKey(new Key(Keys.Escape, true), new KeyAction(new Action(() => { IsExit = true; Exit(); }), new Action(() => { })));
            InputManager.BindKey(new Key(Keys.F11, true), new KeyAction(new Action(() => { ShowDebug = !ShowDebug; }), new Action(() => { })));

            UI = new GUI.UI(this, new System.Drawing.Point(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 288, 0), CelestialBodies[1]);
            UI.Show();

            SpeechRecognizer = new SpeechRecognizer(this);

            //ADD COMMANDS
            SpeechRecognizer.AddCommand("Beende", new Action(() => { SpeechRecognizer.IsRunning = false; Exit(); }));
            SpeechRecognizer.AddCommand("Vogelperspektive", new Action(() =>
            {
                Observer.Camera.SetPosition(new Vector3(Observer.Camera.CameraPosition.X, 3945000.0f, 0));
            }));


            for (int i = 0; i < CelestialBodies.Count; i++)
            {
                var Body = CelestialBodies[i];
                SpeechRecognizer.AddCommand("Markiere " + Body.Name, new Action(() =>
                {
                    Body.SetFocus();
                    UI.SetPlanet(Body);
                }));
                SpeechRecognizer.AddCommand("Folge " + Body.Name, new Action(() =>
                {
                    Body.Follow();
                }));
                SpeechRecognizer.AddCommand("Gehe zu " + Body.Name, new Action(() =>
                {
                    Observer.Camera.SetPosition(new Vector3((float)Body.XCoord, Observer.Camera.CameraPosition.Y, (float)Body.YCoord));
                    Observer.Camera.CameraPosition = new Vector3(Observer.Camera.CameraPosition.X, MathHelper.Lerp(Observer.Camera.CameraPosition.Y, 0, 0.8f), Observer.Camera.CameraPosition.Z);
                }));
            }

            SpeechRecognizer.Run();

            //NetworkAPI = new NetworkAPI(this, "p5cms.esy.es", "P5", "Input.php");
            //NetworkAPI.Start();

            Grid = new Grid3D(GraphicsDevice);
            Grid.MaxColumn = 50;
            Grid.MaxRows = 50;
            Grid.StepHeight = 150000;
            Grid.StepWidth = 150000;
            Grid.Color = new Color(55, 55, 55);
            Grid.CreateGrid();

            MouseInteraction = new MouseInteraction();

        }
        private void LoadCelestialBodies()
        {
            CelestialBodies.Add(new Sun(this, GraphicsDevice, Observer.Camera));

            CelestialBodies.Add(new Merkur(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Venus(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Erde(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Mars(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Jupiter(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Saturn(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Uranus(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Neptun(this, GraphicsDevice, Observer.Camera));
            CelestialBodies.Add(new Pluto(this, GraphicsDevice, Observer.Camera));
            //CelestialBodies.Add(new Halleyscher_Komet(this, GraphicsDevice, Observer.Camera));

            CelestialBody.SetInfluencingBoedies(new CelestialBody[] { 
            CelestialBodies[0],
            CelestialBodies[1], CelestialBodies[2], 
            CelestialBodies[3], CelestialBodies[4], 
            CelestialBodies[5], CelestialBodies[6],
            CelestialBodies[7], CelestialBodies[8], 
            CelestialBodies[9]});

            for (int i = 0; i < CelestialBodies.Count; i++)
            {
                CelestialBodies[i].Begin();
            }
        }
        protected override void UnloadContent()
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ClickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if(_elapsed_time > 1000.0f)
            {
                fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0.0f;
                SunCollisionActive = true;
            }

            InputManager.Update();
            MouseInteraction.Update();

            if (Mouse.GetState().LeftButton == ButtonState.Released)
                LeftButtonPressed = false;

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!RightButtonPressed)
                {
                    Observer.Camera.MouseSensity = .00025f;
                    MousePositionSpace = MouseInteraction.GetXZAtY(MouseInteraction.GetRay(GraphicsDevice,
                                         Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1f, 1000.0f),
                                         Observer.Camera.ViewMatrix),
                                         Observer.Camera.CameraPosition.Y);


                    MousePositionSpace = new Vector3(MousePositionSpace.X - CelestialBody.GLOBALX, MousePositionSpace.Y - CelestialBody.GLOBALY, MousePositionSpace.Z - CelestialBody.GLOBALZ);


                    RightButtonPressed = true;

                }

                Observer.Camera.SetLookAt(MousePositionSpace, Zoom);

            }

            if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                Observer.Camera.MouseSensity = .0008f;
                RightButtonPressed = false;
            }

            if (MouseInteraction.IsScrollingUp)
            {
                Zoom -= 5.5f;
                Observer.Camera.Move(new Vector3(0, 0, -5.5f));
            }
            if (MouseInteraction.IsScrollingDown)
            {
                Zoom += 5.5f;
                Observer.Camera.Move(new Vector3(0, 0, 5.5f));
            }

            Observer.Camera.Update(); 
           
            CelestialBody.GLOBALX = Observer.Camera.CameraPosition.X;
            CelestialBody.GLOBALY = Observer.Camera.CameraPosition.Y;
            CelestialBody.GLOBALZ = Observer.Camera.CameraPosition.Z;

            for (int i = 0; i < CelestialBodies.Count; i++)
            {
                CelestialBodies[i].Update(gameTime);
                if (SunCollisionActive)
                    if (CelestialBodies[i].Name != "Sun")
                        if (CelestialBodies[i].BoundingBox.Intersects(CelestialBodies.Find(p => p.Name == "Sun").BoundingBox))
                            CelestialBodies.RemoveAt(i);
            }
            
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!LeftButtonPressed)
                {
                    LeftButtonPressed = true;
                    Ray r = ToolKit.CalculateRay(new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                        Observer.Camera.ViewMatrix, Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                        GraphicsDevice.Viewport.AspectRatio, 0.01f, 150000.0f),
                        GraphicsDevice.Viewport);

                    if (ClickTimer < TimerDelay)
                    {
                        for (int i = 0; i < CelestialBodies.Count; i++)
                            if (r.Intersects(CelestialBodies[i].BoundingBox) > 1)
                            {
                                Observer.Camera.Flush();
                                UI.SetPlanet(CelestialBodies[i]);
                                CelestialBodies[i].Follow();
                                break;
                            }
                    }
                    else
                    {
                        for (int i = 0; i < CelestialBodies.Count; i++)
                            if (r.Intersects(CelestialBodies[i].BoundingBox) > 1)
                            {
                                UI.SetPlanet(CelestialBodies[i]);
                                CelestialBodies[i].SetFocus();
                                break;
                            }
                    }
                    ClickTimer = 0;
                }
            }


          

            UI.UpdateQR(gameTime);

            CelestialBodies = CelestialBodies.OrderByDescending(p => p.DistanceToObserver).ToList();
        }

        public void SetInputManager(InputManager inputmanager)
        {
            InputManager = inputmanager;
        }

        protected override void Draw(GameTime gameTime)
        {
            //FIX POSTPROCESSING
            spriteBatch.Begin();
            spriteBatch.End();

            BloomComponent.BeginDraw();
            GraphicsDevice.Clear(Color.Black);
            for (int i = 0; i < CelestialBodies.Count; i++)
            {
                if (CelestialBodies[i].RenderWithBloom)
                {
                    CelestialBodies[i].Render();
                }
            }
            base.Draw(gameTime);

            Grid.Render(Observer.Camera.ViewMatrix, Observer.Camera.ProjectionsMatrix, Matrix.CreateTranslation(new Vector3(CelestialBody.GLOBALX - 3500000, CelestialBody.GLOBALY, CelestialBody.GLOBALZ - 3500000)));

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            for (int i = 0; i < CelestialBodies.Count; i++)
            {

                CelestialBodies[i].DrawEllipsoid();
                if (!CelestialBodies[i].RenderWithBloom)
                {
                    spriteBatch.Begin();
                    spriteBatch.End();

                    var v1 = Vector3.Zero;
                    var v2 = new Vector3((float)CelestialBodies[i].XCoord + CelestialBody.GLOBALX, (float)CelestialBodies[i].ZCoord + CelestialBody.GLOBALY, (float)CelestialBodies[i].YCoord + CelestialBody.GLOBALZ);

                    var direction = v2 - v1;
                    direction.Normalize();

                    Ray Ray = new Ray(Vector3.Zero, direction);

                    CelestialBodies[i].IsAllowedToRender = !(Ray.Intersects(CelestialBodies.Find(p => p.Name == "Sun").BoundingBox) > 1) || (v2 - v1).Length() < (new Vector3(CelestialBody.GLOBALX, CelestialBody.GLOBALY, CelestialBody.GLOBALZ) - v1).Length();
                    CelestialBodies[i].Render();
                }
                spriteBatch.Begin();
                CelestialBodies[i].DrawInformation(spriteBatch);
                spriteBatch.End();

                if (ShowDebug)
                    BoundingBoxRenderer.Render(CelestialBodies[i].BoundingBox, GraphicsDevice, Observer.Camera.ViewMatrix, Observer.Camera.ProjectionsMatrix, Color.Red);
            }

            _total_frames++;

            if (ShowDebug)
            {
                spriteBatch.Begin();
                Observer.Camera.RenderDebug(spriteBatch, DebugFont);
                CelestialBody.DrawDebug(spriteBatch, DebugFont);
                spriteBatch.DrawString(DebugFont, fps + " frames per second", new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth - UI.Width - 150, 15), Color.White);
                spriteBatch.End();
            }
        }

        public static Texture2D GetScreenshot(Solarsystem g)
        {
            int w = g.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = g.GraphicsDevice.PresentationParameters.BackBufferHeight;

            //force a frame to be drawn (otherwise back buffer is empty) 
            g.Draw(new GameTime());

            //pull the picture from the buffer 
            int[] backBuffer = new int[w * h];
            g.GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture 
            Texture2D texture = new Texture2D(g.GraphicsDevice, w, h, false, g.GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);
            return texture;
        }
    }
}