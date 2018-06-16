using System.IO;
using System.Windows;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string path;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0) //make sure an argument is passed
            {
                FileInfo file = new FileInfo(e.Args[0]);
                if (file.Exists) //make sure it's actually a file
                    path = e.Args[0];
            }
        }
    }
}
