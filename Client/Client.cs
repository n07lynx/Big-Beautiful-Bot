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
            var hostInfo = Dns.GetHostEntry("localhost");
            var address = hostInfo.AddressList.First();
            var endpoint = new IPEndPoint(address, 662);
            socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);

            var inThread = new Thread(ReadInputs);
            inThread.Start();
            var outThread = new Thread(DisplayOutputs);
            outThread.Start();

            inThread.Join();
            outThread.Join();
        }

        public static void ReadInputs()
        {
            while(true)
            {
                var value = Console.ReadLine();
                socket.Send(Encoding.Unicode.GetBytes(value));
            }
        }

        public static void DisplayOutputs()
        {
            while(true)
            {
                var bytes = new byte[1024];
                var readBytes = socket.Receive(bytes);
                Console.WriteLine(Encoding.Unicode.GetString(bytes,0,readBytes));
            }
        }

    }
}