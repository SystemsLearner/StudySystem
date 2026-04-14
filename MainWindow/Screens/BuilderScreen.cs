using Microsoft.Win32;
using StudySystem.Controls;
using StudySystem.Core.JCard;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        // Button Methods
        private void SaveDeckButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            SelectedEditorDeck = selectedDeck;
            UpdateEditorCard();
            if (selectedDeck == null || selectedDeck.Cards.Count == 0)
            {
                MessageBox.Show("No deck selected.");
                return;
            }
            SaveDeckToFile(selectedDeck);
            MessageBox.Show("Deck saved.");
        }

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

        private void BuilderFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshBuilderPreview();
        }

        private void BuilderDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            if (selectedDeck == null)
            {
                BuilderScreen.CardComboBoxControl.ItemsSource = null;
                return;
            }
            for (int i = 0; i < selectedDeck.Cards.Count; i++)
            {
                selectedDeck.Cards[i].Index = i + 1;
            }
            BuilderScreen.CardComboBoxControl.ItemsSource = null;
            BuilderScreen.CardComboBoxControl.ItemsSource = selectedDeck.Cards;
        }

        private void BuilderCardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card card = BuilderScreen.CardComboBoxControl.SelectedItem as Card ?? new Card();
            BuilderScreen.FrontTextBoxControl.Text = card.Front ?? "";
            BuilderScreen.ReadingTextBoxControl.Text = card.Reading ?? "";
            BuilderScreen.PronunciationTextBoxControl.Text = card.Pronunciation ?? "";
            BuilderScreen.AnswerTextBoxControl.Text = card.Answer ?? "";
            RefreshBuilderPreview();
        }

        // Non-Button Methods
        private void RefreshBuilderPreview()
        {
            BuilderScreen.EditorCardViewControl.SetCard(new Card
            {
                Front = BuilderScreen.FrontTextBoxControl.Text,
                Reading = BuilderScreen.ReadingTextBoxControl.Text,
                Pronunciation = BuilderScreen.PronunciationTextBoxControl.Text,
                Answer = BuilderScreen.AnswerTextBoxControl.Text
            });
        }

        private void RefreshEditorDeckSelection()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = null;
            BuilderScreen.DeckComboBoxControl.ItemsSource = MainDecks;
            BuilderScreen.DeckComboBoxControl.DisplayMemberPath = "DisplayName";
        }

        private void UpdateEditorCard()
        {
            SelectedEditorCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (SelectedEditorCard != null)
            {
                BuilderScreen.EditorCardViewControl.UpdateCard(SelectedEditorCard);
            }
        }

        private void EditorLoadDecksIntoComboBox()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = MainDecks;
        }

        private void SaveDeckToFile(Deck deck)
        {
            if (deck == null)
            {
                MessageBox.Show("No deck selected.");
                return;
            }
            string folder = _IOLogic.GetDecksFolder();
            string fileName = deck.Name.Replace(" ", "") + ".jcard";
            fileName = fileName.Replace("_", "");
            string path = System.IO.Path.Combine(folder, fileName);
            _IOLogic.WriteDeck(deck, path);
            EditorLoadDecksIntoComboBox();
        }
    }
}
