using System.Diagnostics;
using System.Windows.Media;

namespace Vocabulary.Logic.Generic
{
    internal static class Executer
    {
        #region Contants
        internal const string MSSTORELINK = "https://www.microsoft.com/store/p/vocabulary/9nbbfrhb33g9", BARBEZEULINK = "https://barbez.eu", LAUNCHER = "explorer.exe";
        #endregion

        internal static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>
        /// Executes an application location at a path, using parameters.
        /// </summary>
        /// <param name="appPath">The location where the application is located.</param>
        /// <param name="args">The arguments that will be passed to the application.</param>
        /// <param name="useShellExecute">Set to true to use shell execution; set to false if not.</param>
        /// <param name="waitForExit">Set to true if need to wait for the application to finish execution.</param>
        internal static void RunExternalProcess(string appPath, string args, bool useShellExecute, bool waitForExit)
        {
            ProcessStartInfo info = new ProcessStartInfo(appPath, @args)
            {
                UseShellExecute = useShellExecute
            };

            Process p = new Process
            {
                StartInfo = info
            };
            p.Start();
            if (waitForExit) p.WaitForExit();
        }

    }
}