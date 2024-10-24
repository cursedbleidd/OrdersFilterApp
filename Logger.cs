using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersFilterApp
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            File.AppendAllText(_logFilePath, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} INFO: {message}\n");
        }
        public void LogError(string message)
        {
            File.AppendAllText(_logFilePath, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ERROR: {message}\n");
        }
    }
}
