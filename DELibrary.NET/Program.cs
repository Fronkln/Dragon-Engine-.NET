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
           
        }

        //Do whatever you want here
        static void ThreadTest()
        {
            //Not doing this will crash the game if you try anything
            DragonEngine.Initialize();       

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
