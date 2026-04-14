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
            RefreshEditorCardSelection();
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

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            if (selectedDeck == null)
            {
                MessageBox.Show("No deck selected.");
                return;
            }
            Card newCard = new Card
            {
                Front = "",
                Reading = "",
                Pronunciation = "",
                Answer = ""
            };
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedCard == null || selectedDeck.Cards.Count == 0)
            {
                selectedDeck.Cards.Add(newCard);
            }
            else
            {
                for (int i = 0; i < selectedDeck.Cards.Count; i++)
                {
                    if (selectedDeck.Cards[i] == selectedCard)
                    {
                        selectedDeck.Cards.Insert(i + 1, newCard);
                        break;
                    }
                }
            }
            for (int u = 0; u < selectedDeck.Cards.Count; u++)
            {
                selectedDeck.Cards[u].Index = u + 1;
            }
            BuilderScreen.CardComboBoxControl.ItemsSource = null;
            BuilderScreen.CardComboBoxControl.ItemsSource = selectedDeck.Cards;
            BuilderScreen.CardComboBoxControl.SelectedItem = newCard;
            UpdateCardNavigationButtons();
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedDeck == null)
            { MessageBox.Show("No deck selected."); return; }
            if (selectedCard == null || selectedDeck.Cards.Count == 0)
            { MessageBox.Show("No card selected."); return; }
            int removedIndex = selectedDeck.Cards.IndexOf(selectedCard);
            if (removedIndex == -1)
            { MessageBox.Show("Selected card was not found."); return; }
            selectedDeck.Cards.RemoveAt(removedIndex);
            for (int i = 0; i < selectedDeck.Cards.Count; i++)
            { selectedDeck.Cards[i].Index = i + 1; }
            BuilderScreen.CardComboBoxControl.ItemsSource = null;
            BuilderScreen.CardComboBoxControl.ItemsSource = selectedDeck.Cards;
            if (selectedDeck.Cards.Count == 0) {
                BuilderScreen.CardComboBoxControl.SelectedItem = null;
                BuilderScreen.FrontTextBoxControl.Text = "";
                BuilderScreen.ReadingTextBoxControl.Text = "";
                BuilderScreen.PronunciationTextBoxControl.Text = "";
                BuilderScreen.AnswerTextBoxControl.Text = "";
                RefreshBuilderPreview(); return; }
            if (removedIndex < selectedDeck.Cards.Count)
            { BuilderScreen.CardComboBoxControl.SelectedItem = selectedDeck.Cards[removedIndex]; }
            else
            { BuilderScreen.CardComboBoxControl.SelectedItem = selectedDeck.Cards[removedIndex - 1]; }
            UpdateCardNavigationButtons();
        }

        private void NextCard_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedDeck == null || selectedDeck.Cards.Count == 0)
            { MessageBox.Show("No deck selected."); return; }
            if (selectedCard == null)
            { BuilderScreen.CardComboBoxControl.SelectedIndex = 0; return; }
            int currentIndex = selectedDeck.Cards.IndexOf(selectedCard);
            if (currentIndex == -1) { return; }
            if (currentIndex < selectedDeck.Cards.Count - 1)
            { BuilderScreen.CardComboBoxControl.SelectedIndex = currentIndex + 1; }
        }

        private void PreviousCard_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedDeck == null || selectedDeck.Cards.Count == 0)
            { MessageBox.Show("No deck selected."); return; }
            if (selectedCard == null) { BuilderScreen.CardComboBoxControl.SelectedIndex = 0; return; }
            int currentIndex = selectedDeck.Cards.IndexOf(selectedCard);
            if (currentIndex == -1) { return; }
            if (currentIndex > 0) { BuilderScreen.CardComboBoxControl.SelectedIndex = currentIndex - 1; }
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
            UpdateCardNavigationButtons();
        }

        private void BuilderCardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card card = BuilderScreen.CardComboBoxControl.SelectedItem as Card ?? new Card();
            BuilderScreen.FrontTextBoxControl.Text = card.Front ?? "";
            BuilderScreen.ReadingTextBoxControl.Text = card.Reading ?? "";
            BuilderScreen.PronunciationTextBoxControl.Text = card.Pronunciation ?? "";
            BuilderScreen.AnswerTextBoxControl.Text = card.Answer ?? "";
            RefreshBuilderPreview();
            UpdateCardNavigationButtons();
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

        private void RefreshEditorCardSelection()
        {
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            RefreshEditorDeckSelection();
            BuilderScreen.DeckComboBoxControl.SelectedItem = selectedDeck;
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
            BuilderScreen.CardComboBoxControl.SelectedItem = selectedCard;
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

        private void SetPreviousVisible(bool visible)
        {
            if (visible)
            {
                // If true ...
                BuilderScreen.PreviousCardButtonControl.Opacity = 1.0;
                BuilderScreen.PreviousCardButtonControl.IsEnabled = true;
            }
            else
            {
                // If false ...
                BuilderScreen.PreviousCardButtonControl.Opacity = 0.4;
                BuilderScreen.PreviousCardButtonControl.IsEnabled = false;
            }
        }

        private void SetNextVisible(bool visible)
        {
            if (visible)
            {
                // If true ...
                BuilderScreen.NextCardButtonControl.Opacity = 1.0;
                BuilderScreen.NextCardButtonControl.IsEnabled = true;
            }
            else
            {
                // If false ...
                BuilderScreen.NextCardButtonControl.Opacity = 0.4;
                BuilderScreen.NextCardButtonControl.IsEnabled = false;
            }
        }

        private void UpdateCardNavigationButtons()
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedDeck == null || selectedDeck.Cards.Count == 0 || selectedCard == null)
            {
                SetPreviousVisible(false);
                SetNextVisible(false);
                return;
            }
            int index = selectedDeck.Cards.IndexOf(selectedCard);
            SetPreviousVisible(index > 0);
            SetNextVisible(index < selectedDeck.Cards.Count - 1);
        }
    }
}
