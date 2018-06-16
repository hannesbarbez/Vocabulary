using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Vocabulary.Logic;
using Vocabulary.Logic.Generic;

namespace Vocabulary
{
    public partial class WindowMain : Window
    {
        #region Fields
        private DictionaryDelegate wordlistDelegate;
        private Dictionary dictionary;
        private List<int> indicesNotToUseAnymore;
        private string previousWord = "";
        private int index = -1;
        #endregion

        #region Misc. UI Logic
        private void SetDictionary(Dictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private MessageBoxResult AskQuestions(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.datagrid.HasItems)
            {
                MessageBoxResult mbr = MessageBox.Show("Do you wish to save any changes made to this dictionary?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (mbr)
                {
                    case MessageBoxResult.Yes:
                        BtnSaveAs_Click(null, null);
                        return mbr;
                    case MessageBoxResult.No:
                        return mbr;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        return mbr;
                }
            }
            return MessageBoxResult.None;
        }

        private MessageBoxResult AskQuestions()
        {
            if (this.datagrid.HasItems)
            {
                MessageBoxResult mbr = MessageBox.Show("Do you wish to save any changes made to this dictionary?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (mbr)
                {
                    case MessageBoxResult.Yes:
                        BtnSaveAs_Click(null, null);
                        return mbr;
                    case MessageBoxResult.No:
                        return mbr;
                    case MessageBoxResult.Cancel:
                        return mbr;
                    default:
                        return mbr;
                }
            }
            return MessageBoxResult.None;
        }

        private int GenerateRandomIndex()
        {
            return new Random().Next(0, this.dictionary.Wordlist.Count);
        }

        private bool IndexHasBeenUsed(int i)
        {
            if (this.indicesNotToUseAnymore.Count > 0 && this.indicesNotToUseAnymore.Count() != this.dictionary.Wordlist.Count())
                foreach (int j in this.indicesNotToUseAnymore)
                    if (j == i)
                        return true;
            return false;
        }

        private void ChooseNextWord()
        {
            if ((bool)cbRandomize.IsChecked)
            {
                this.index = GenerateRandomIndex();

                while (IndexHasBeenUsed(this.index))
                    this.index = GenerateRandomIndex();
            }
            else this.index++;

            //show the correct answer to previous word
            this.lblPreviousChallenge.Content = this.previousWord;

            //Don't give a new challenge when all has been asked
            if (this.indicesNotToUseAnymore.Count != this.dictionary.Wordlist.Count)
            {
                //avoid the same challenge to be asked again later (todo: make this into a radiobutton option to maybe allow this)
                this.indicesNotToUseAnymore.Add(this.index);

                //select the word at the generated index
                Word w = this.dictionary.Wordlist[this.index];

                //question this word in the right direction
                if ((bool)rb0to1.IsChecked)
                {
                    lblCurrentChallenge.Content = w.Language0;
                    this.previousWord = w.Language1;
                }
                else
                {
                    lblCurrentChallenge.Content = w.Language1;
                    this.previousWord = w.Language0;
                }
            }
            else { lblCurrentChallenge.Content = ""; }
        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = Executer.GetVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    datagrid.ScrollIntoView(rowContainer, datagrid.Columns[column]);
                    presenter = Executer.GetVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                datagrid.UpdateLayout();
                datagrid.ScrollIntoView(datagrid.Items[index]);
                row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        #endregion

        #region UI Handler Methods
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = AskQuestions();
            if (mbr != MessageBoxResult.Cancel)
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Vocabulary++ XML files (*.vocab)|*.vocab"
                };
                ofd.ShowDialog();

                if (File.Exists(ofd.FileName))
                {
                    this.dictionary = HelperClass.Open(ofd.FileName);
                    DoStuffUponOpenFile();
                }
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            new WindowAbout().ShowDialog();
        }

        private void BtnExam_Click(object sender, RoutedEventArgs e)
        {

                if (this.dictionary != null && this.dictionary.Wordlist.Count > 0)
                {
                    tiExamTest.Visibility = Visibility.Visible;
                    tcMainWindow.SelectedIndex = 1;
                    this.indicesNotToUseAnymore = new List<int>();
                    this.index = -1;
                }
        }

        private void BtnNextChallenge_Click(object sender, RoutedEventArgs e)
        {
            this.datagrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (this.datagrid.Items.Count > 0)
                ChooseNextWord();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = AskQuestions();
            if (mbr != MessageBoxResult.Cancel)
            {
                new WindowLanguages(this.wordlistDelegate).ShowDialog();
                this.index = -1;
                tiExamTest.Visibility = Visibility.Hidden;
                VisualizeDictionary();
            }
        }

        private void VisualizeDictionary()
        {
            if (this.dictionary != null)
            {
                this.datagrid.ItemsSource = this.dictionary.Wordlist;
                this.datagrid.Columns[0].Header = this.dictionary.Language0;
                this.datagrid.Columns[1].Header = this.dictionary.Language1;
                this.datagrid.Columns[0].MinWidth = 100;
                this.datagrid.Columns[1].MinWidth = 100;

                this.cbRandomize.IsChecked = this.dictionary.Randomize;
                if (this.dictionary.Question0to1) rb0to1.IsChecked = true;
                else rb1to0.IsChecked = true;

                this.rb0to1.Content = (string)this.dictionary.Language0 + " to " + (string)this.dictionary.Language1;
                this.rb1to0.Content = (string)this.dictionary.Language1 + " to " + (string)this.dictionary.Language0;
            }
        }

        private void DoStuffUponOpenFile()
        {
            if (this.dictionary.Language0 == "" | this.dictionary.Language1 == "") this.dictionary = null;
            tiExamTest.Visibility = Visibility.Hidden;
            this.index = -1;
            VisualizeDictionary();
        }

        private void BtnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            this.datagrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (this.datagrid.Items.Count > 0 && !string.IsNullOrWhiteSpace(this.dictionary.Language0) && !string.IsNullOrWhiteSpace(this.dictionary.Language1))
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = datagrid.Columns[0].Header + "-" + datagrid.Columns[1].Header,
                    AddExtension = true,
                    Filter = "Vocabulary XML files (*.vocab)|*.vocab",
                    OverwritePrompt = true
                };
                sfd.ShowDialog();

                this.dictionary.Randomize = (bool)this.cbRandomize.IsChecked;
                if ((bool)rb0to1.IsChecked) this.dictionary.Question0to1 = true;
                else this.dictionary.Question0to1 = false;

                HelperClass.Save(sfd.FileName, this.dictionary);
            }
        }

        private void Datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 2)
            {
                int currentRowIndex = this.datagrid.ItemContainerGenerator
                .IndexFromContainer(this.datagrid.ItemContainerGenerator.ContainerFromItem(this.datagrid.CurrentItem));

                if (currentRowIndex > 0)
                {
                    if ((GetCell(currentRowIndex - 1, 0).Content as TextBlock).Text == "")
                    {
                        GetCell(currentRowIndex - 1, 0).Focus();
                        GetRow(currentRowIndex - 1).IsSelected = true;
                        GetRow(currentRowIndex).IsSelected = false;
                    }
                    else if ((GetCell(currentRowIndex - 1, 1).Content as TextBlock).Text == "")
                    {
                        GetCell(currentRowIndex - 1, 1).Focus();
                        GetRow(currentRowIndex - 1).IsSelected = true;
                        GetRow(currentRowIndex).IsSelected = false;
                    }
                    else if ((GetCell(currentRowIndex, 0).Content as TextBlock).Text == "")
                    {
                        GetCell(currentRowIndex, 0).Focus();
                        GetRow(currentRowIndex).IsSelected = true;
                    }
                }
            }
        }

        private void CbRandomize_Checked(object sender, RoutedEventArgs e)
        {
            if (this.dictionary != null)
                this.dictionary.Randomize = true;
        }

        private void CbRandomize_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.dictionary != null)
                this.dictionary.Randomize = false;
        }

        private void TukajWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AskQuestions(sender, e);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            lblCurrentChallenge.Content = "";
            lblPreviousChallenge.Content = "";
            BtnExam_Click(null, null);
            BtnNextChallenge_Click(null, null);
        }

        private void LblAbout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.MSSTORELINK);
        }

        private void LblAbout_MouseDown(object sender, TouchEventArgs e)
        {
            HelperClass.RunExternalProcess(Executer.LAUNCHER, Executer.MSSTORELINK);
        }
        #endregion

        #region Constructors
        public WindowMain()
        {
            InitializeComponent();

            this.wordlistDelegate = new DictionaryDelegate(this.SetDictionary);

            if (App.path != null)
                if (!string.IsNullOrEmpty(App.path) | !string.IsNullOrWhiteSpace(App.path))
                {
                    this.dictionary = HelperClass.Open(App.path);
                    DoStuffUponOpenFile();
                }

        }
        #endregion
    }
}