using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.HttpClientWrap
{
    public class Logger : INoty
    {
        public void Error(string? title = null, string? message = null)
        {
            Log(title, message);
        }

        public void Log(string? title = null, string? message = null)
        {
#if DEBUG
            Console.WriteLine($"[{title}] {message}");
#endif
        }
    }

    public interface INoty
    {
        void Log(string? title = null, string? message = null);
        void Error(string? title = null, string? message = null);
    }
}
