using System;

namespace CS4300_Final_Project
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FinalProject game = new FinalProject())
            {
                game.Run();
            }
        }
    }
#endif
}

