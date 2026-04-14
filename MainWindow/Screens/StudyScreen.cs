using Microsoft.Win32;
using StudySystem.Core.JCard;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void DeckStudyButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;

            if (selectedDeck == null || selectedDeck.Cards.Count == 0) { return; }

            StudyDeck.StartDeck(selectedDeck);

            UpdateCardScreen();
            ShowScreen(CardScreen);
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

        private void StudyDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;
        }
    }
}
