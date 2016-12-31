using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rTunes.Common
{
    public interface ILogger
    {
        void Log(string message, string sender = "");
    }
    public class Logger : ILogger
    {
        private LoggerWindow _wnd;

        public Logger()
        {
            TextWriterTraceListener fileLog = new TextWriterTraceListener("log.txt");
            Trace.Listeners.Add(fileLog);

            _wnd = new LoggerWindow();
            _wnd.Show();
        }
        public void Log(string message, string sender = "")
        {
            _wnd.WriteLog(message, sender);
            Trace.TraceInformation(message);
            Trace.Flush();
        }
    }
}
