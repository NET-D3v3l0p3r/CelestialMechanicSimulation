//‏بسم الله الرحمن الرحيم
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
using System.Diagnostics;
using CelestialMechanicsSimulator.Core.Maths;
using CelestialMechanicsSimulator.Core.InstancedCircle;
using System.Numerics;
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{
    public class CelestialBody
    {

        public static readonly double ASTRONOMICUNIT = 149597870700;
        public static readonly double KEPLERCONSTANTSUN = 2.97f * 0.0000000000000000001f; // 2.97*10^(-19) s^2/m^3
        public static readonly double GRAVITATIONCONSTANT = 6.67408e-11;
        public static readonly double SUN_G = 1.989E30 * GRAVITATIONCONSTANT;
        public static readonly double Exponent = 3;

        public static float GLOBALSCALE = 1000.0f;
        
        public static float GLOBALX;
        public static float GLOBALY;
        public static float GLOBALZ;

        private static bool BUSY;
        private static string PLANET;

        public static SpriteFont DEBUGFONT;

        public Solarsystem Handler { get; private set; }
        public GraphicsDevice Device { get; private set; }

        public Camera Camera { get; set; }

        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public Color TypicalColor { get; private set; }
        public bool IsAllowedToRender { get; set; }

        public bool RenderWithBloom { get; set; }

        public string ModelPath { get; set; }
        public string TexturePath { get; set; }

        public string Name { get; set; }

        public float Scale { get; set; }
        public float Size { get; set; }
        public float Rotation { get; set; }

        public double XCoord { get; set; }
        public double YCoord { get; set; }
        public double ZCoord { get; set; }

        public double AbsoluteX { get; set; }
        public double AbsoluteY { get; set; }
        public double AbsoluteZ { get; set; }

        public double AccelerationX { get; private set; }
        public double AccelerationY { get; private set; }
        public double AccelerationZ { get; private set; }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double VelocityZ { get; set; }

        public double Distance { get; private set; }
        public int Sign { get; set; }

        public double DeltaTime { get; set; }

        public float CenterX { get; set; }
        public float CenterY { get; set; }

        public double PlanetMassConst { get; private set; }
        public double PlanetMass { get; set; }

        public long Scope { get; private set; }
        public float TurnAroundTime { get; set; }
        public double Time { get; private set; }

        public float LinearEccentricity { get; private set; }
        public float NumericEccentricity { get; set; }

        public float Periphel { get; set; }
        public float Aphel { get; set; }
        public float MajorAxisA { get; set; }
        public float MinorAxisB { get; set; }

        public float DistanceToSun { get; private set; }
        public float DistanceToObserver { get; set; }

        public BoundingBox BoundingBox { get; private set; }
        public Matrix World { get; private set; }

        public static CelestialBody[] InfluencingBodies { get; private set; }

        protected CelestialBodyType @Type;
        protected InstancedEllipse InstancedEllipse;
        protected float PhysicsScale;
        protected double PortionY, PortionZ;

        private bool IsInFocus;
        private bool IsFollowed;

        public CelestialBody(Solarsystem handler, GraphicsDevice device, Camera camera)
        {
            Handler = handler;
            Device = device;

            Camera = camera;

            TexturePath = @"Textures\" + this.GetType().Name + "Color";
            ModelPath = @"CelestialBody\celestialbody";

            Name = this.GetType().Name;

            Sign = 1;
        }

        public virtual void LoadContent()
        {
            var CLoader = Handler.Content;

            Model = CLoader.Load<Model>(ModelPath);
            Texture = CLoader.Load<Texture2D>(TexturePath);

            InfluencingBodies = new CelestialBody[0];

            PlanetMassConst = PlanetMass;

            //TODO: TRY TO GET REQUIRED VALUES FROM WIKIPEDIA(!)
        }

        /*TODO: CALCULATE ELLIPOSID ..: */

        public void Begin()
        {
            Calculate();
        }

        public virtual void Calculate()
        {
            MajorAxisA = (float)ToAstronomicUnit(Math.Pow((Math.Pow(TurnAroundTime, 2) / KEPLERCONSTANTSUN), 1.0 / 3));
            LinearEccentricity = NumericEccentricity * MajorAxisA;
            MinorAxisB = (float)Math.Sqrt(Math.Pow(MajorAxisA, 2) - Math.Pow(LinearEccentricity, 2));
            Periphel = MajorAxisA - LinearEccentricity;
            Aphel = MajorAxisA + LinearEccentricity;
            Scope = (long)(Math.PI * Math.Sqrt(2 * (Math.Pow(ToOriginDistance(MajorAxisA), 2) + Math.Pow(ToOriginDistance(MinorAxisB), 2))));
            CenterX = LinearEccentricity * GLOBALSCALE;
            Scale = (float)((Size / 12800) * 65);


            /*~~~INIT~~~*/

            XCoord = ToOriginDistance(MajorAxisA);
            Distance = XCoord;

            AbsoluteX = XCoord;
            AbsoluteY = YCoord;
            AbsoluteZ = ZCoord;
            var Velocity = Math.Sqrt(SUN_G * (2 / ToOriginDistance(MajorAxisA * 1) - (1 / ToOriginDistance(MajorAxisA * 1))));
            VelocityY = Velocity * PortionY;
            VelocityX = Velocity * (1 - PortionY);
            VelocityZ = Velocity * PortionZ;

            AccelerationX = AccelerationY = AccelerationZ = 0;


            /*~~~END~~~*/

            if (this.Type == CelestialBodyType.Focus)
                return;

            Stopwatch benchmark = new Stopwatch();
            benchmark.Start();
            GetEllipsoidColor();
            if(Solarsystem.SIMULATOR_TYPE == Solarsystem.SIMULATOR_MODE.SIMULATOR_SOLARSYSTEM)
                InstancedEllipse.CalculateEllipsoid(Name, AbsoluteX, AbsoluteY, AbsoluteZ, VelocityX, VelocityY, VelocityZ, DeltaTime, TurnAroundTime * 1.5, PhysicsScale, Exponent); 
            //InstancedEllipse.CalculateEllipsoid(CenterX, CenterY, MajorAxisA, MinorAxisB, GLOBALSCALE, 0.001f);
            benchmark.Stop();
            Console.WriteLine(benchmark.Elapsed.TotalSeconds + "s to generate ellipsoid by color[" + Name + "]");

            DeltaTime = 60 * 60 * 24 * 7 * 0.01;

            Handler.InputManager.BindKey(new Key(Keys.Up, true), new KeyAction(new Action(() => {
                if (this.DeltaTime * 1.5 < 1065588.166015625)
                    this.DeltaTime *= 1.5; 
            }), new Action(() => { })));
            Handler.InputManager.BindKey(new Key(Keys.Down, true), new KeyAction(new Action(() => { this.DeltaTime /= 1.5; }), new Action(() => { })));

        }

        private void GetEllipsoidColor()
        {
             
            Color[] __chace = new Color[Texture.Width * Texture.Height];
            Color[] __copier = new Color[255 + 255 + (255 * 1)];
            int[] __chace1 = new int[255 + 255 + (255 * 1)];
             
            Func<byte, byte, byte, int, long> hash = (byte r__in, byte g__in, byte b__in, int n) =>
            {
                return r__in + g__in + (b__in * 0); ;
            };

            Texture.GetData<Color>(__chace);

            int __index = 0;
            int __max = __chace1[__index];
            for (int j = 0; j < Texture.Height; j++)
            {
                for (int i = 0; i < Texture.Width; i++)
                {
                    var c = __chace[i + j * Texture.Width];
                    var h = hash(c.R, c.G, c.B, Texture.Width * Texture.Height);
                    __chace1[h]++;
                    __copier[h] = c;
                    if (__chace1[h] > __max)
                    {
                        __max = __chace1[h];
                        __index = (int)h;
                    }
                }
            }

            TypicalColor = __copier[__index];

            InstancedEllipse.EllipsoidColor = TypicalColor;



        }
        public virtual void Update(GameTime gTime)
        {
            if (@Type == CelestialBodyType.Planet)
            {
                Rotation += Sign * (0.001f * (float)gTime.ElapsedGameTime.TotalMilliseconds);

                for (int i = 0; i < InfluencingBodies.Length; i++)
                {
                    if (InfluencingBodies[i].Name != Name)
                    {
                        Distance = Math.Sqrt(Math.Pow((AbsoluteX - (InfluencingBodies[i].AbsoluteX)), 2) + Math.Pow((AbsoluteY - (InfluencingBodies[i].AbsoluteY)), 2) + Math.Pow((AbsoluteZ - (InfluencingBodies[i].AbsoluteZ)), 2));

                        AccelerationX += (-InfluencingBodies[i].PlanetMass * GRAVITATIONCONSTANT) * (AbsoluteX / (Math.Pow(Distance, Exponent)));
                        AccelerationY += (-InfluencingBodies[i].PlanetMass * GRAVITATIONCONSTANT) * (AbsoluteY / (Math.Pow(Distance, Exponent)));
                        AccelerationZ += (-InfluencingBodies[i].PlanetMass * GRAVITATIONCONSTANT) * (AbsoluteZ / (Math.Pow(Distance, Exponent)));
                    }   
                }

                VelocityX += AccelerationX * DeltaTime;
                VelocityY += AccelerationY * DeltaTime;
                VelocityZ += AccelerationZ * DeltaTime;

                AbsoluteX += (VelocityX * DeltaTime);
                AbsoluteY += (VelocityY * DeltaTime);
                AbsoluteZ += (VelocityZ * DeltaTime);

                XCoord = AbsoluteX / PhysicsScale;
                YCoord = AbsoluteY / PhysicsScale;
                ZCoord = AbsoluteZ / PhysicsScale;

                AccelerationX = AccelerationY = AccelerationZ = 0;

                DistanceToSun = (float)ToAstronomicUnit(Math.Sqrt(Math.Pow(AbsoluteX - 0, 2) + Math.Pow(AbsoluteY - 0, 2) + Math.Pow(AbsoluteZ - 0, 2)));

                switch (Solarsystem.SIMULATOR_TYPE)
                {
                    case Solarsystem.SIMULATOR_MODE.SIMULATOR_SOLARSYSTEM:
                        if (Camera.CameraMoving)
                            InstancedEllipse.Update(gTime);
                        break;

                    case Solarsystem.SIMULATOR_MODE.SIMULATOR_THREE_BODY_P:
                        InstancedEllipse.AddTransformation(new EllipseTransformations(new Vector3((float)XCoord, (float)ZCoord, (float)YCoord)));
                        InstancedEllipse.Update(gTime);

                        if (Time / 100000 > TurnAroundTime / 24 / 60 / 60)
                        {
                            Time = 0;
                            InstancedEllipse.Flush();
                        }

                        Time += DeltaTime;
                        break;
                }

                DistanceToObserver = new Vector3((float)this.XCoord + GLOBALX, (float)ZCoord + GLOBALY, (float)this.YCoord + GLOBALZ).Length();
            }
        }

        public static void SetInfluencingBoedies(CelestialBody[] planets)
        {
            InfluencingBodies = planets;
        }

        public virtual void Render()
        {
            Matrix[] boneTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * World;
                    BoundingBox = BoundingBoxRenderer.UpdateBoundingBox(Model, effect.World);
                    effect.View = Camera.ViewMatrix;
                    effect.Projection = Camera.ProjectionsMatrix;
                    effect.Texture = Texture;
                    effect.TextureEnabled = true;
                    if (@Type == CelestialBodyType.Focus)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.SpecularPower = 5.0f;
                        effect.AmbientLightColor = Color.DarkRed.ToVector3();
                        effect.SpecularColor = Vector3.One;
                    }
                    else
                    {
                        effect.LightingEnabled = false;
                        effect.AmbientLightColor = Color.Black.ToVector3();
                    }
                    effect.CurrentTechnique.Passes[0].Apply();
                }
                if (Camera.ViewFrustum.Contains(BoundingBox) != ContainmentType.Disjoint && IsAllowedToRender)
                    mesh.Draw();
            }
        }

        public void DrawInformation(SpriteBatch sBatch)
        {
            if (@Type == CelestialBodyType.Planet)
            {
                if (IsInFocus)
                {
                    for (int i = 0; i < InfluencingBodies.Length; i++)
                    {
                        if (InfluencingBodies[i].Name != Name)
                        {
                            LineBatchTester.LineBatch.DrawLine(sBatch, Color.Red, ToolKit.Project(Device, Camera.ProjectionsMatrix,
                                Camera.ViewMatrix, Matrix.Identity,
                                new Vector3((float)this.XCoord + GLOBALX, (float)this.ZCoord + GLOBALY, (float)this.YCoord + GLOBALZ)),
                   ToolKit.Project(Device, Camera.ProjectionsMatrix, Camera.ViewMatrix, Matrix.Identity,
                   new Vector3(
                       (float)(0 - -InfluencingBodies[i].XCoord) + GLOBALX,
                       (float)(0 - -InfluencingBodies[i].ZCoord) + GLOBALY,
                       (float)(0 - -InfluencingBodies[i].YCoord) + GLOBALZ)));
                        }
                    }

                    sBatch.DrawString(DEBUGFONT, Name + Environment.NewLine + "{X}: " + XCoord + Environment.NewLine +
                                  "{Y}: " + YCoord +
                                  Environment.NewLine + "{DISTANCE_TO_SUN}: " + DistanceToSun + "AE" +
                                  Environment.NewLine + "{DISTANCE_TO_OBSERVER}: " + DistanceToObserver +
                    Environment.NewLine + "{VELOCITY_X}: " + VelocityX +
                    Environment.NewLine + "{VELOCITY_Y}: " + VelocityY +
                    Environment.NewLine + "TIME: " + Time / 100000, ToolKit.Project(Device, Camera.ProjectionsMatrix, Camera.ViewMatrix, World, new Vector3(-6.739463f, 10.43321f, -25.85147f)), Color.Red);
                }
                else
                    sBatch.DrawString(DEBUGFONT, Name, ToolKit.Project(Device, Camera.ProjectionsMatrix, Camera.ViewMatrix, World, new Vector3(-6.739463f, 10.43321f, -25.85147f)), Color.Red);

            }
        }

        public void DrawEllipsoid()
        {
            if (InstancedEllipse != null)
                InstancedEllipse.Render();
        }

        public enum CelestialBodyType
        {
            Focus,
            Planet
        }

        public void SetWorldMatrix(Matrix world)
        {
            World = world;
        }
        public void SetFocus()
        {
            IsInFocus = !IsInFocus;
        }

        public void Follow()
        {
            if (!BUSY)
            {
                PLANET = Name;
                IsFollowed = true;
                BUSY = true;
                Camera.MouseSensity = .00025f;
            }
            else if (BUSY && PLANET == Name)
            {
                IsFollowed = false;
                BUSY = false;
                PLANET = "";
                Camera.MouseSensity = .0008f;
            }
        }

        public virtual void DrawSatellitePathAndSatellite() { }

        public static double ToAstronomicUnit(double value)
        {
            return value / ASTRONOMICUNIT;
        }

        public static double ToOriginDistance(double value)
        {
            return value * ASTRONOMICUNIT;
        }

        public static float CalculateHypotenuse(Vector2 v1, Vector2 v2)
        {
            var a = v1.Y > v2.Y ? v1.Y - v2.Y : v2.Y - v1.Y;
            var b = v1.X > v2.X ? v1.X - v2.X : v2.X - v1.X;
            var c = (float)(Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)));
            return c;
        }

        public static void DrawDebug(SpriteBatch sbatch, SpriteFont font)
        {
            for (int i = 0; i < InfluencingBodies.Length; i++)
                sbatch.DrawString(font, InfluencingBodies[i].Name + ": " + Environment.NewLine  + "--------" + Environment.NewLine + "POSX: " + (InfluencingBodies[i].XCoord + GLOBALX) + Environment.NewLine +
                    "POSY: " + (InfluencingBodies[i].ZCoord + GLOBALY) + Environment.NewLine +
                    "POSZ: " + (InfluencingBodies[i].YCoord + GLOBALZ) + Environment.NewLine + "--------", new Vector2(0, 95 + 5 + i * 95), Color.DarkGray);
        }
    }
}