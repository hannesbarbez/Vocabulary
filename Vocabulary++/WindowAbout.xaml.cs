// Copyright (c) Hannes Barbez. All rights reserved.
// Licensed under the GNU General Public License v3.0

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
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
            Title = "About " + assemblyName.Name;
            tbDescription.Text = description.Description;
            tbVersion.Text = "Version " + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
            tbCopyright.Text = $"©2017-{DateTime.Now.Year} Barbez.eu.";
        }

        private void HelpWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }

        private void HelpWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        #endregion

        private void BtnRate_Click(object sender, RoutedEventArgs e)
        {
            Executer.RateApp();
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e)
        {
            Executer.VisitGithub();
        }
    }
}
