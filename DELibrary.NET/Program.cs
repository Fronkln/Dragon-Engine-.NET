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
            bool isQKeyPressed = DragonEngine.IsKeyDown(81);

            //This isn't inconsistent though!
            bool isQKeyBeingHeld = DragonEngine.IsKeyHeld(81);
        }

        //Do whatever you want here
        static void ThreadTest()
        {
            DragonEngine.Log("Thread start");

            //Not doing this will crash the game if you try anything
            DragonEngine.Initialize();

            //This function will execute every frame DE does!
            DragonEngine.RegisterJob(Update, DEJob.Update);

            //Create enemy at player position
            Character player = DragonEngine.GetHumanPlayer();
            FighterManager.GenerateEnemyFighter(new PoseInfo(player.GetPosCenter(), 31), 0x3C8C, CharacterID.m_kiryu);
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
