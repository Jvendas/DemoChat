using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.helpers
{
    public class LogHelper
    {
        private static string logFilename = "ChatLog.txt";

        public static void logToFile(string msg)
        {
            msg = DateTime.Now + " - " + msg;
            Console.WriteLine(msg);

            FileStream fs = new FileStream(logFilename, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(msg);
            sw.Close();
            fs.Close();
        }
    }
}
