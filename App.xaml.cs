using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Threading;

namespace StarWarsFilterApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
    }
}

