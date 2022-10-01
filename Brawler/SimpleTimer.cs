using System;
using System.Timers;

namespace Brawler
{
    /// <summary>
    /// Simple one time timer
    /// </summary>
    public class SimpleTimer
    {
        public SimpleTimer(float seconds, Action func)
        {
            Timer tim = new Timer();
            tim.Interval = TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            tim.Enabled = true;
            tim.AutoReset = false;
            tim.Elapsed += delegate { func(); };
        }
    }   
}
