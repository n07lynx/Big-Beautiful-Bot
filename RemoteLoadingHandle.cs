using System;
using System.Net.Sockets;

namespace BigBeautifulBot.Input
{
    internal class RemoteLoadingHandle : IDisposable
    {
        private Socket _Connection;

        public bool IsLoading;

        public RemoteLoadingHandle(Socket connection)
        {
            _Connection = connection;
            IsLoading = true;
        }

        public void Dispose()
        {
            IsLoading = false;
        }
    }
}