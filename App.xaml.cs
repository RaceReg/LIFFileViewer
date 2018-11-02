using Avalonia;
using Avalonia.Markup.Xaml;

namespace LIFFileViewer
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}