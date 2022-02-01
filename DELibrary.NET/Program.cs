using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Input;


namespace DragonEngineLibrary
{
    public class MyClass
    {
        static void Update()
        {

            //Warning: This function is inconsistent
            //You might need to press the key a few times for it to work.
            //This is a problem with attaching to the Update function of the game which does not execute as frequent as the key func
            //I will find a solution for it later.
            //You might want to use this function inside a while loop outside of a Update function for consistency
            //ThreadTest maybe?
            bool isQKeyPressed = DragonEngine.IsKeyDown(VirtualKey.Q);

            //This isn't inconsistent though!
            bool isQKeyBeingHeld = DragonEngine.IsKeyHeld(VirtualKey.Q);
        }

        //Do whatever you want here
        static void ThreadTest()
        {
            DragonEngine.Log("Dragon Engine Library .NET Thread Start");

            while (!DragonEngine.IsEngineInitialized())
            {
                continue;
            }

            DragonEngine.Log("Engine initialized");

            //GameDir
            DragonEngine.InitializeModLibrary("MyTestMod.dll");
            //  DragonEngine.InitializeModLibrary("Yakuza 7 Online.dll");

        }

        // This method will be called by native code inside the target process…
        public static void Main(string[] args)
        {
            //Initialize logging file
            File.Create("dotnetlog.txt").Close();
            DragonEngine._logStream = new MemoryStream();
            DragonEngine._logWriter = new StreamWriter(DragonEngine._logStream);

            //Create seperate thread for our C# library
            Thread thread1 = new Thread(ThreadTest);
            thread1.Start();
        }
    }
}
