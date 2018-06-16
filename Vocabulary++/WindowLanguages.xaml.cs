using System;
using System.Windows;
using Vocabulary.Logic;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for LanguagesWindow.xaml
    /// </summary>
    public partial class WindowLanguages : Window
    {
        private DictionaryDelegate wld;

        public WindowLanguages()
        {
            InitializeComponent();
        }

        public WindowLanguages(DictionaryDelegate wld)
        {
            InitializeComponent();
            this.wld = wld;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbLanguage1.Text.Length > 0 && tbLanguage2.Text.Length > 0 && !string.IsNullOrWhiteSpace(tbLanguage1.Text) && !string.IsNullOrWhiteSpace(tbLanguage2.Text))
            {
                this.wld(new Dictionary(tbLanguage1.Text.Trim(), tbLanguage2.Text.Trim()));
                Close();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbLanguage1.Focus();
        }
    }
}
