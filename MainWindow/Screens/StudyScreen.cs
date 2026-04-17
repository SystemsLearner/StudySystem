using Microsoft.Win32;
using StudySystem.Core.JCard;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using StudySystem.Screens;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
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
            currentCardsInDeckPosition = 1;
            currentDeckCardsCount = selectedDeck.Cards.Count;
            if (selectedDeck == null || selectedDeck.Cards == null || selectedDeck.Cards.Count == 0) { return; }

            Deck studyDeck = BuildStudySession(selectedDeck);
            StudyDeck.StartDeck(studyDeck);
        }

        private Deck BuildStudySession(Deck selectedDeck)
        {
            if (selectedDeck == null || selectedDeck.Cards == null)
            {
                _studySessionCards = new List<Card>();
                _studySessionIndex = 0;
                return null;
            }
            _studySessionCards = selectedDeck.Cards
                .OrderBy(GetStudyPriority)
                .ToList();
            _studySessionIndex = 0;
            return selectedDeck;
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
