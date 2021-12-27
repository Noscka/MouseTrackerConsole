using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Drawing;

namespace SprayReader
{
    class Program
    {
        private static List<Coords> QuickCoords = new List<Coords>();
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo("SprayImage");
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

            if (!Directory.Exists("SprayImage"))
            {
                Directory.CreateDirectory("SprayImage");
            }

            string DirectoryOfFiles = args[0];
            try 
            {
                int number = 0;
                foreach (string file in Directory.GetFiles(DirectoryOfFiles, ".spray"))
                {
                    QuickCoords = JsonConvert.DeserializeObject<List<Coords>>(File.ReadAllText(file));
                    DrawingSpray(QuickCoords, number);
                    QuickCoords.Clear();
                    number++;
                }
                

            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory Not Found");
                Console.WriteLine(DirectoryOfFiles);
                Thread.Sleep(-1);
            }
            Thread.Sleep(-1);
        }

        private static void DrawingSpray(List<Coords> LinesCoords, int filenumber)
        {
            int ResolutionX = Screen.PrimaryScreen.Bounds.Width;
            int ResolutionY = Screen.PrimaryScreen.Bounds.Height;


            Bitmap bmp = new Bitmap(ResolutionX, ResolutionY);
            Pen blackPen = new Pen(Color.Black, 3);

            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, ResolutionX, ResolutionY));
                int index = 0;
                foreach (var coord in LinesCoords)
                {
                    if (index == 0)
                    {

                    }
                    else
                    {
                        graphics.DrawLine(blackPen, float.Parse(LinesCoords[index-1].X), float.Parse(LinesCoords[index-1].Y), float.Parse(coord.X), float.Parse(coord.Y));
                    }
                    index++;
                }
            }

            bmp.Save($@"SprayImage\Spray{filenumber}.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
    internal class Coords
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
