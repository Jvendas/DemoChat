using System;
using System.IO;

namespace ChatServer.helpers
{
    public class LogHelper
    {
        private static string logFilename = "ServerLog.txt";

        //Função generica de guardar dados processados no servidor e cliente
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
