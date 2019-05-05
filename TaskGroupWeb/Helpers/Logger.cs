using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace TaskGroupWeb.Helpers
{
    public class Logger
    {
        public static void SaveLog(Exception e, IConfiguration configuration)
        {
            var logPath = configuration.GetSection("dirLog").Value;
            var fileName = DateTime.Now.ToShortDateString().Replace("/", "") + ".txt";

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);                
            }

            var filePath = Path.Combine(logPath, fileName);

            using (StreamWriter w = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                w.WriteLine($"{DateTime.Now.TimeOfDay} - [Source: {e.Source}] [Message: {e.Message}]");
                w.WriteLine("");
            }
        }
    }
}
