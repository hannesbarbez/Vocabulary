// Copyright (c) Hannes Barbez. All rights reserved.
// Licensed under the GNU General Public License v3.0

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
        private readonly DictionaryDelegate wordlistDelegate;
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
            if (datagrid.HasItems)
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
            if (datagrid.HasItems)
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
            return new Random().Next(0, dictionary.Wordlist.Count);
        }

        private bool IndexHasBeenUsed(int i)
        {
            if (indicesNotToUseAnymore.Count > 0 && indicesNotToUseAnymore.Count() != dictionary.Wordlist.Count())
                foreach (int j in indicesNotToUseAnymore)
                    if (j == i)
                        return true;
            return false;
        }

        private void ChooseNextWord()
        {
            if ((bool)cbRandomize.IsChecked)
            {
                index = GenerateRandomIndex();

                while (IndexHasBeenUsed(this.index))
                    index = GenerateRandomIndex();
            }
            else index++;

            //show the correct answer to previous word
            lblPreviousChallenge.Content = previousWord;

            //Don't give a new challenge when all has been asked
            if (indicesNotToUseAnymore.Count != dictionary.Wordlist.Count)
            {
                //avoid the same challenge to be asked again later // TODO: make this into a radiobutton option to maybe allow this in future.
                indicesNotToUseAnymore.Add(index);

                //select the word at the generated index
                Word w = dictionary.Wordlist[index];

                //question this word in the right direction
                if ((bool)rb0to1.IsChecked)
                {
                    lblCurrentChallenge.Content = w.Language0;
                    previousWord = w.Language1;
                }
                else
                {
                    lblCurrentChallenge.Content = w.Language1;
                    previousWord = w.Language0;
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
                    dictionary = HelperClass.Open(ofd.FileName);
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

            if (dictionary != null && dictionary.Wordlist.Count > 0)
            {
                tiExamTest.Visibility = Visibility.Visible;
                tcMainWindow.SelectedIndex = 1;
                indicesNotToUseAnymore = new List<int>();
                index = -1;
            }
        }

        private void BtnNextChallenge_Click(object sender, RoutedEventArgs e)
        {
            datagrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (datagrid.Items.Count > 0)
                ChooseNextWord();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = AskQuestions();
            if (mbr != MessageBoxResult.Cancel)
            {
                new WindowLanguages(wordlistDelegate).ShowDialog();
                index = -1;
                tiExamTest.Visibility = Visibility.Hidden;
                VisualizeDictionary();
            }
        }

        private void VisualizeDictionary()
        {
            if (this.dictionary != null)
            {
                datagrid.ItemsSource = dictionary.Wordlist;
                datagrid.Columns[0].Header = dictionary.Language0;
                datagrid.Columns[1].Header = dictionary.Language1;
                datagrid.Columns[0].MinWidth = 100;
                datagrid.Columns[1].MinWidth = 100;

                cbRandomize.IsChecked = dictionary.Randomize;
                if (dictionary.Question0to1) rb0to1.IsChecked = true;
                else rb1to0.IsChecked = true;

                rb0to1.Content = dictionary.Language0 + " to " + dictionary.Language1;
                rb1to0.Content = dictionary.Language1 + " to " + dictionary.Language0;
            }
        }

        private void DoStuffUponOpenFile()
        {
            if (dictionary.Language0 == "" | dictionary.Language1 == "") dictionary = null;
            tiExamTest.Visibility = Visibility.Hidden;
            index = -1;
            VisualizeDictionary();
        }

        private void BtnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            datagrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (datagrid.Items.Count > 0 && !string.IsNullOrWhiteSpace(dictionary.Language0) && !string.IsNullOrWhiteSpace(dictionary.Language1))
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = datagrid.Columns[0].Header + "-" + datagrid.Columns[1].Header,
                    AddExtension = true,
                    Filter = "Vocabulary XML files (*.vocab)|*.vocab",
                    OverwritePrompt = true
                };
                sfd.ShowDialog();

                dictionary.Randomize = (bool)cbRandomize.IsChecked;
                dictionary.Question0to1 = (bool)rb0to1.IsChecked;
                HelperClass.Save(sfd.FileName, dictionary);
            }
        }

        private void Datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 2)
            {
                int currentRowIndex = datagrid.ItemContainerGenerator.IndexFromContainer(datagrid.ItemContainerGenerator.ContainerFromItem(datagrid.CurrentItem));
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
            if (dictionary != null)
                dictionary.Randomize = true;
        }

        private void CbRandomize_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dictionary != null)
                dictionary.Randomize = false;
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
        #endregion

        #region Constructors
        public WindowMain()
        {
            InitializeComponent();
            wordlistDelegate = new DictionaryDelegate(SetDictionary);
            if (App.path != null)
                if (!string.IsNullOrEmpty(App.path) | !string.IsNullOrWhiteSpace(App.path))
                {
                    dictionary = HelperClass.Open(App.path);
                    DoStuffUponOpenFile();
                }
        }
        #endregion
    }
}
