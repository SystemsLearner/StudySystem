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
        private Card _previousSelectedCard;
        private bool _isLoadingCard;

        // Button Methods
        private void SaveDeckButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            SelectedEditorDeck = selectedDeck;
            SaveCurrentBuilderCardEdits();
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
                Extras = "",
                Pronunciation = "",
                Answer = "",
                Difficulty = Card.CardResult.Unsure
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
            if (_isLoadingCard)
                return;
            _isLoadingCard = true;
            if (selectedDeck.Cards.Count == 0) {
                BuilderScreen.CardComboBoxControl.SelectedItem = null;
                BuilderScreen.FrontFieldControl.InputText = "";
                BuilderScreen.ReadingFieldControl.InputText = "";
                BuilderScreen.ExtrasFieldControl.InputText = "";
                BuilderScreen.PronunciationFieldControl.InputText = "";
                BuilderScreen.AnswerFieldControl.InputText = "";
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
            if (_isLoadingCard)
                return;
            RefreshBuilderPreview();
        }

        private void BuilderDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoadingCard)
                return;
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
            if (_isLoadingCard)
                return;
            _isLoadingCard = true;
            if (_previousSelectedCard != null)
            {
                _previousSelectedCard.Front = BuilderScreen.FrontFieldControl.InputText;
                _previousSelectedCard.Reading = BuilderScreen.ReadingFieldControl.InputText;
                _previousSelectedCard.Extras = BuilderScreen.ExtrasFieldControl.InputText;
                _previousSelectedCard.Pronunciation = BuilderScreen.PronunciationFieldControl.InputText;
                _previousSelectedCard.Answer = BuilderScreen.AnswerFieldControl.InputText;
            }
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedCard != null)
            {
                BuilderScreen.FrontFieldControl.InputText = selectedCard.Front ?? "";
                BuilderScreen.ReadingFieldControl.InputText = selectedCard.Reading ?? "";
                BuilderScreen.ExtrasFieldControl.InputText = selectedCard.Extras ?? "";
                BuilderScreen.PronunciationFieldControl.InputText = selectedCard.Pronunciation ?? "";
                BuilderScreen.AnswerFieldControl.InputText = selectedCard.Answer ?? "";
            }
            else
            {
                BuilderScreen.FrontFieldControl.InputText = "";
                BuilderScreen.ReadingFieldControl.InputText = "";
                BuilderScreen.ExtrasFieldControl.InputText = "";
                BuilderScreen.PronunciationFieldControl.InputText = "";
                BuilderScreen.AnswerFieldControl.InputText = "";
            }
            _isLoadingCard = false;
            _previousSelectedCard = selectedCard;
            RefreshBuilderPreview();
            UpdateCardNavigationButtons();
        }

        // Non-Button Methods
        private void RefreshBuilderPreview()
        {
            BuilderScreen.EditorCardViewControl.SetCard(new Card
            {
                Front = BuilderScreen.FrontFieldControl.InputTextBoxControl.Text,
                Reading = BuilderScreen.ReadingFieldControl.InputTextBoxControl.Text,
                Extras = BuilderScreen.ExtrasFieldControl.InputTextBoxControl.Text,
                Pronunciation = BuilderScreen.PronunciationFieldControl.InputTextBoxControl.Text,
                Answer = BuilderScreen.AnswerFieldControl.InputTextBoxControl.Text
            });
        }

        private void SaveCurrentBuilderCardEdits()
        {
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (selectedCard == null)
                return;
            if (_isLoadingCard)
                return;
            _isLoadingCard = true;
            selectedCard.Front = BuilderScreen.FrontFieldControl.InputText;
            selectedCard.Reading = BuilderScreen.ReadingFieldControl.InputText;
            selectedCard.Extras = BuilderScreen.ExtrasFieldControl.InputText;
            selectedCard.Pronunciation = BuilderScreen.PronunciationFieldControl.InputText;
            selectedCard.Answer = BuilderScreen.AnswerFieldControl.InputText;
            BuilderScreen.CardComboBoxControl.Items.Refresh();
        }

        private void RefreshEditorDeckSelection()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = null;
            BuilderScreen.DeckComboBoxControl.ItemsSource = MainDecks;
            BuilderScreen.DeckComboBoxControl.DisplayMemberPath = "DisplayName";
        }

        private void RefreshEditorCardSelection()
        {
            RefreshEditorDeckSelection();
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
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
