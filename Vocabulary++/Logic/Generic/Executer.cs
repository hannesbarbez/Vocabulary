using System.Diagnostics;
using System.Windows.Media;

namespace Vocabulary.Logic.Generic
{
    internal static class Executer
    {
        #region Constants
        internal static string MSSTORERATINGURL = "ms-windows-store://review/?productid=9nbbfrhb33g9", BARBEZEULINK = "https://barbez.eu", LAUNCHER = "explorer.exe";
        #endregion

        internal static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default;
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
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

        internal static void RateApp()
        {
            Process.Start(MSSTORERATINGURL);
        }

        internal static void VisitSite()
        {
            Process.Start(BARBEZEULINK);
        }
    }
}
