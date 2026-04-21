using Microsoft.Win32;
using StudySystem.Core.JCard;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using StudySystem.Screens;
using StudySystem.Core;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private List<Card> _studySessionCards;

        // Button Methods
        private void DeckStudyButton_Click(object sender, RoutedEventArgs e)
        {
            StartStudySession();
            ShowScreen(CardScreen);
            UpdateCardScreen();
        }

        private void StudyDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;
        }

        // Non-Button Methods
        private void StartStudySession()
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;
            if (selectedDeck == null || selectedDeck.Cards == null || selectedDeck.Cards.Count == 0) {
                return;
            }
            currentCardsInDeckPosition = 1;
            currentDeckCardsCount = selectedDeck.Cards.Count;
            _studySessionCards = selectedDeck.Cards
                .OrderBy(c => GetStudyPriority(c))
                .ToList();
            StudyDeck.StartDeck(selectedDeck);
        }

        private void RefreshStudyDeckSelection()
        {
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = null;
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = MainDecks;
            StudyScreen.DeckSelectionComboBoxControl.DisplayMemberPath = "Name";
        }

        private void LoadDecksFromDisk()
        {
            MainDecks = _IOLogic.LoadAllDecks();
        }
    }
}
