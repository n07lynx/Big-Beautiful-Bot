using System;
using System.Net.Sockets;

namespace BigBeautifulBot.Input
{
    internal class RemoteLoadingHandle : IDisposable
    {
        public bool IsLoading;

        public RemoteLoadingHandle()
        {
            IsLoading = true;
        }

        public void Dispose()
        {
            IsLoading = false;
        }
    }
}