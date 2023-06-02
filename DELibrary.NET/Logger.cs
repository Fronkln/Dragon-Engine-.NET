using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonEngineLibrary
{
    public class Logger
    {
        public enum Event
        {
            DEBUG,
            INFORMATION,
            WARNING,
            ERROR,
            FATAL,
        }

        private static List<string> EventStrings = new List<string>()
        {
            "DBG",
            "INF",
            "WRN",
            "ERR",
            "FTL",
        };

        private static List<uint> EventColors = new List<uint>()
        {
            0x00000000, //DEBUG
            0x00000000, //INFORMATION
            0x8A0070EE, //WARNING
            0x800000EE, //ERROR
            0xFF00000A, //FATAL
        };

        internal static uint GetColorForLogEventType(Event eventType)
        {
            return EventColors[(int)eventType];
        }


        public struct LogMessage
        {
            public string Source;
            public DateTime Time;
            public Event Event;
            public string Message;


            public string GetLogInfoString()
            {
                return $"{Time.ToString("HH:mm:ss.fff")} | {EventStrings[(int)Event]} | [{Source}]";
            }

            public override string ToString()
            {
                return $"{GetLogInfoString()} {Message}";
            }
        }


        public static volatile ConcurrentQueue<LogMessage> Logs = new ConcurrentQueue<LogMessage>();


        internal static void LogLineEvent(string text, Event eventType, string source)
        {
            LogMessage message = new LogMessage()
            {
                Source = source,
                Time = DateTime.Now,
                Event = eventType,
                Message = text
            };
            Logs.Enqueue(message);
            Console.WriteLine(message.ToString());
        }


        internal static List<LogMessage> GetFilteredLogs(string source, List<Event> logEvents)
        {
            List<LogMessage> filtered = new List<LogMessage>();
            List<LogMessage> logCopy = new List<LogMessage>(Logs);
            var enumer = logCopy.GetEnumerator();
            enumer.MoveNext();
            do
            {
                LogMessage logMessage = enumer.Current;
                if (source == string.Empty || logMessage.Source == source)
                {
                    if (logEvents.Count == 0)
                    {
                        filtered.Add(logMessage);
                    }
                    else
                    {
                        if (logEvents.Contains(logMessage.Event))
                        {
                            filtered.Add(logMessage);
                        }
                    }
                }
            }
            while (enumer.MoveNext());
            return filtered;
        }
    }
}
