using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Vocabulary.Logic;
using Vocabulary.Logic.Generic;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class WindowAbout : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs the Help Window.
        /// </summary>
        public WindowAbout()
        {
            InitializeComponent();
        }
        #endregion

        #region UI Handler Methods
        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            var description = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            var copyright = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).OfType<AssemblyCopyrightAttribute>().FirstOrDefault();

            if (description != null && copyright != null)
            {
                this.Title = "About " + assemblyName.Name + ".";
                tbDescription.Text = description.Description;
                tbVersion.Text = "Version " + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
                tbCopyright.Text = "Copyright " + copyright.Copyright;
            }
        }

        private void HelpWindow_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }

        private void HelpWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void LblAbout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.MSSTORELINK);
        }

        private void LblAbout_MouseDown(object sender, TouchEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.MSSTORELINK);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.BARBEZEULINK);
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.BARBEZEULINK);
        }
        #endregion
    }
}