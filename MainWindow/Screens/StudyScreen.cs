using Microsoft.Win32;
using StudySystem.Core.JCard;
using System.Windows;
using System.IO;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void DeckStudyButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;

            if (selectedDeck == null || selectedDeck.Cards.Count == 0) { return; }

            _logic.StartDeck(selectedDeck);

            UpdateCardScreen();
            ShowScreen(CardScreen);
        }

        private void RefreshStudyDeckSelection()
        {
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = null;
            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = _decks;
            StudyScreen.DeckSelectionComboBoxControl.DisplayMemberPath = "Name";
        }

        private void LoadDecksFromDisk()
        {
            _decks = _IOLogic.LoadAllDecks();
        }
    }
}
