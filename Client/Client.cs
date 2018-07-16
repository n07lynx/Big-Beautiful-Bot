using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using System.Linq;

namespace BigBeautifulBot.Client
{
    public class Program
    {
        static Socket socket;
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            using(var client = new TcpClient("127.0.0.1",662))
            {
                using(var stream = client.GetStream())
                {
                    new Thread(() =>
                    {
                        while(true)
                        {
                            var buffer = new byte[1024];
                            var readValues = stream.Read(buffer, 0, buffer.Length);
                            if(readValues > 0)
                            {
                                Console.WriteLine(Encoding.Unicode.GetString(buffer, 0, readValues));
                            }
                        }
                    }).Start();

                    while(true)
                    {
                        var ibn = Console.ReadLine();
                        stream.Write(Encoding.Unicode.GetBytes(ibn));
                    }
                }
            }
        }
    }
}