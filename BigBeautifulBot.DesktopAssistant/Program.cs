using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace BigBeautifulBot.DesktopAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>();
        }

        static Socket socket;
        static void Old(string[] args)
        {
            using (var client = new TcpClient("127.0.0.1", 662))
            {
                using (var stream = client.GetStream())
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            var buffer = new byte[1024];
                            var readValues = stream.Read(buffer, 0, buffer.Length);
                            if (readValues > 0)
                            {
                                Console.WriteLine(Encoding.Unicode.GetString(buffer, 0, readValues));
                            }
                        }
                    }).Start();

                    while (true)
                    {
                        var ibn = Console.ReadLine();
                        stream.Write(Encoding.Unicode.GetBytes(ibn), 0, ibn.Length);
                    }
                }
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
