// Copyright (c) Hannes Barbez. All rights reserved.
// Licensed under the GNU General Public License v3.0

using System.Diagnostics;
using System.Windows.Media;

namespace Vocabulary.Logic.Generic
{
    internal static class Executer
    {
        #region Constants
        private static string MSSTORERATINGURL = "ms-windows-store://review/?productid=9nbbfrhb33g9", BARBEZEULINK = "https://barbez.eu", GITHUBLINK = "https://github.com/hannesbarbez/Vocabulary";
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

        internal static void VisitGithub()
        {
            Process.Start(GITHUBLINK);
        }
    }
}
