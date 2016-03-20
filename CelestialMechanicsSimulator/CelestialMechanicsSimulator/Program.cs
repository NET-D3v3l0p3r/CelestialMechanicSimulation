using System;

namespace CelestialMechanicsSimulator
{
#if WINDOWS || XBOX
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (Solarsystem game = new Solarsystem())
            {
                game.Run();
            }
        }
    }
#endif
}

