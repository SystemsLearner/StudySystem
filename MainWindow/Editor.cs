using StudySystem.Core.JCard;
using System.Windows;
using System.Windows.Controls;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void SaveDeckButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = BuilderScreen.DeckComboBoxControl.SelectedItem as Deck;
            SelectedEditorDeck = selectedDeck;
            SaveCard();
            if (selectedDeck == null || selectedDeck.Cards.Count == 0)
            {
                MessageBox.Show("No deck selected.");
                return;
            }
            string folder = _IOLogic.GetDecksFolder();
            string fileName;

            fileName = selectedDeck.Name.Replace(" ", "") + ".jcard";
            fileName = fileName.Replace("_", "");
            string path = System.IO.Path.Combine(folder, fileName);

            _IOLogic.WriteDeck(selectedDeck, path);
            EditorLoadDecksIntoComboBox();
            MessageBox.Show("Deck saved.");
        }

        private void SaveCard()
        {
            SelectedEditorCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;
            if (SelectedEditorCard != null)
            {
                BuilderScreen.EditorCardViewControl.UpdateCard(SelectedEditorCard);
            }
        }

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
    }
}
