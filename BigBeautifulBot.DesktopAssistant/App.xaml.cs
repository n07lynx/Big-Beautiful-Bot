using Avalonia;
using Avalonia.Markup.Xaml;

namespace BigBeautifulBot.DesktopAssistant
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}