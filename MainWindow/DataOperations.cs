using Microsoft.Win32;
using StudySystem.Core.JCard;
using System;
using System.IO;
using System.Windows;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void ImportDeckButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JCard Files (*.jcard)|*.jcard",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string sourcePath = dialog.FileName;
            string folder = _IOLogic.GetDecksFolder();

            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(folder, fileName);


            try
            {
                if (File.Exists(destPath))
                {
                    MessageBox.Show("A deck with this name already exists." +
                        "\r\n\r\nPlease choose a different name or replace the existing file.");
                }
                else if (!File.Exists(destPath))
                {
                    File.Copy(sourcePath, destPath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occured: \n" + ex.Message);
            }

            LoadDecksFromDisk();
            RefreshEditorDeckSelection();
        }

        private void ImportDecksButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JCard Files (*.jcard)|*.jcard",
                Multiselect = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string destFolder = _IOLogic.GetDecksFolder();

            foreach (string sourcePath in dialog.FileNames)
            {
                string fileName = System.IO.Path.GetFileName(sourcePath);
                string destPath = System.IO.Path.Combine(destFolder, fileName);

                File.Copy(sourcePath, destPath, true);
            }
            MessageBox.Show("Deck(s) imported successfully.");
            LoadDecksFromDisk();
            RefreshEditorDeckSelection();
        }

        private void CreateTemplateDeck_Click(object sender, RoutedEventArgs e)
        {
            Deck templateDeck = new Deck();
            templateDeck.Name = "Template Deck";

            templateDeck.Cards.Add(new Card
            {
                Front = "食べる",
                Reading = "たべる",
                Pronunciation = "",
                Answer = "to eat"
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "飲む",
                Reading = "のむ",
                Pronunciation = "",
                Answer = "to drink"
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "見る",
                Reading = "みる",
                Pronunciation = "",
                Answer = "to see"
            });

            string folder = _IOLogic.GetDecksFolder();
            string fileName = templateDeck.Name.Replace(" ", "") + ".jcard";
            fileName = fileName.Replace("_", "");
            string path = System.IO.Path.Combine(folder, fileName);
            try
            {
                _IOLogic.WriteDeck(templateDeck, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error happened: \n" + ex.Message);
            }

            LoadDecksFromDisk();
            RefreshStudyDeckSelection();
        }

        private void LoadDecksFromDisk()
        {
            _decks = _IOLogic.LoadAllDecks();
        }

        private void EditorLoadDecksIntoComboBox()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = _decks;
        }

        private void RefreshStudyDeckSelection()
        {
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = null;
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = _decks;
            StudyScreen.DeckSelectionComboBoxControl.DisplayMemberPath = "Name";
        }

        private void RefreshEditorDeckSelection()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = null;
            BuilderScreen.DeckComboBoxControl.ItemsSource = _decks;
            BuilderScreen.DeckComboBoxControl.DisplayMemberPath = "DisplayName";
        }

        private void SaveAllDecksButton_Click(object sender, RoutedEventArgs e)
        {
            _IOLogic.SaveAllDecks(_decks);
        }
    }
}
