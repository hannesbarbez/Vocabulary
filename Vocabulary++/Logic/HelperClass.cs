using System.Diagnostics;
using Vocabulary.Data;

namespace Vocabulary.Logic
{
    internal class HelperClass
    {
        private const string NOTONWINDOWS10S = "Unfortunately, this feature is not available on Windows 10 S.";

        internal static bool Save(string path, Dictionary dictionary)
        {
            return DiskIO.Save(path, dictionary);
        }

        internal static Dictionary Open(string path)
        {
            return DiskIO.Open(path);
        }

        internal static void RunExternalProcess(string appPath, string args)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(appPath, @args)
                {
                    UseShellExecute = false
                };

                Process p = new Process
                {
                    StartInfo = info
                };
                p.Start();
            }
            catch
            {
                //TODO: Say why this does not work under Windows 10 S
            }
        }
    }
}