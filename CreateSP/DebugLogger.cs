using System;
using System.IO;

namespace CreateSp
{
    public static class DebugLogger
    {
        public static void log(string DebugMessage)
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string ApplicationPath = Path.GetFullPath(Path.Combine(CurrentDirectory, @"..\..\..\..\")); // zelfde path als de .sln file 
            string SavePath = ApplicationPath.ToString() + @"\debuglog.txt";            
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(SavePath, append: true))
            {
                sw.WriteLine(DateTime.Now.ToString() + " | " + DebugMessage);
            }
        }
    }
}