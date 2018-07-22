using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace BigBeautifulBot.DesktopAssistant
{
    public class MainWindow : Window, IDisposable
    {
        public NetworkStream BotTcpClientStream { get; }
        public static MainWindow Current { get; private set; }

        BBBViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new BBBViewModel();
            BotTcpClientStream = new TcpClient("132.148.82.115", 662).GetStream();
            SynchronizationContext.Current.Post(StreamListener, null);
            Current = this;
        }

        private async void StreamListener(object state)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var readBuffer = new byte[1024];
                    var byteCount = BotTcpClientStream.Read(readBuffer, 0, readBuffer.Length);
                    if (byteCount <= 0) continue;

                    var incomingMessage = Encoding.Unicode.GetString(readBuffer, 0, byteCount);
                    ViewModel.OutputMessages += $"BBB: {incomingMessage}\n";
                }
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                byte[] message = Encoding.Unicode.GetBytes(ViewModel.InputBoxText);
                BotTcpClientStream.Write(message, 0, message.Length);
                ViewModel.OutputMessages += $"YOU: {ViewModel.InputBoxText}\n";
                ViewModel.InputBoxText = string.Empty;
            }
        }

        public void Dispose()
        {
            BotTcpClientStream.Dispose();
        }
    }

    public class BBBViewModel : ReactiveObject
    {
        private string _inputBoxText;
        private string _outputText;

        public string OutputMessages { get => _outputText; set => _outputText = this.RaiseAndSetIfChanged(ref _outputText, value); }
        public string InputBoxText { get => _inputBoxText; set => _inputBoxText = this.RaiseAndSetIfChanged(ref _inputBoxText, value); }
    }
}