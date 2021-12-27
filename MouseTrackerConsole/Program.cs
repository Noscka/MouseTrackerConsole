using System;
using Timer = System.Timers.Timer;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Gma.System.MouseKeyHook;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MouseTrackerConsole
{
    class Program
    {
        private static List<Coords> QuickCoords = new List<Coords>();
        private static int fileNumber = 0;
        private static Rectangle screenResolution = Screen.PrimaryScreen.Bounds;
        private static IKeyboardMouseEvents HookEvents = null;
        private static Timer loopTimer;

        [STAThread]
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo("Spray");

            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (DirectoryNotFoundException)
            {

            }

            loopTimer = new Timer();
            loopTimer.Interval = 10;
            loopTimer.Enabled = false;
            loopTimer.Elapsed += loopTimerEvent;
            loopTimer.AutoReset = true;


            if (!Directory.Exists("Spray"))
            {
                Directory.CreateDirectory("Spray");
            }

            Hook.GlobalEvents().MouseDown += async (sender, e) => /// Checks for mouse down input (checks for any mouse button)
            {
                if (e.Button == MouseButtons.Left) /// checks if the mouse down is left mouse
                {
                    loopTimer.Enabled = true; ///enables a loop that records the mouse Co ords
                }
            };

            Hook.GlobalEvents().MouseUp += async (sender, e) => /// Checks for mouse up input (checks for any mouse button)
            {
                if (e.Button == MouseButtons.Left) /// checks if the mouse up is left mouse
                {
                    loopTimer.Enabled = false; ///disables the loop
                    if (QuickCoords.Count <= 10) /// if the coords are less then 10 then it doesnt write it into file as it is most likely a click
                    {

                    }
                    else 
                    {
                        using (StreamWriter file = File.CreateText($@"Spray\Spray{fileNumber}.spray")) /// sets up the writing to file which, filenumber is used to be able to store many files
                        {
                            JsonSerializer serializer = new JsonSerializer(); /// serialized to json
                            serializer.Serialize(file, QuickCoords); /// writes the list to the spray file using json format
                        }
                        QuickCoords.Clear(); /// clears the list
                        fileNumber += 1; 
                    }
                    
                }
            };
            try
            {
                Application.Run(new ApplicationContext());
            }
            catch (AccessViolationException)
            {
                Environment.Exit(0);
            }
        }
        private static void loopTimerEvent(Object source, ElapsedEventArgs e) /// the loop that records the mouse movement
        {
            Console.WriteLine("x: " + Cursor.Position.X + " y: " + Cursor.Position.Y); /// shows the coords in console for debug
            QuickCoords.Add(new Coords { X = Cursor.Position.X.ToString(), Y = Cursor.Position.Y.ToString() }); /// adds the coords to the list
        }
    }

    internal class Coords /// object for the list
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
