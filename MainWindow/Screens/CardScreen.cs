using StudySystem.Controls;
using StudySystem.Core;
using StudySystem.Core.JCard;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudySystem
{
    public partial class MainWindow : Window
    {

        private DeckIO deckIO = new DeckIO();
        private int currentCardsInDeckPosition = 1;
        private int currentDeckCardsCount = 0;
        private int totalCardsShownCount = 0;
        private string diffOfCard = "";
        private bool _showSavedDifficultySelection = false;
        
        // Button Methods
        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            CardScreen.MainCardViewControl.SetAnswerVisible(true);//Toggles the AnswerText on/off

            if (GlobalSettings.AutoAdvanceAfterAnswer)
            {
                CardScreen.ShowAnswerButtonControl.Visibility = Visibility.Collapsed;
                NextCard();
                _showSavedDifficultySelection = true;
            }
            CardScreen.ShowAnswerButtonControl.Visibility = Visibility.Collapsed;
            CardScreen.NextCardButtonControl.Visibility = Visibility.Visible;
            CardScreen.NextCardButtonControl.IsEnabled = true;
            CardScreen.NextCardButtonControl.Opacity = 1.0;
            _showSavedDifficultySelection = true;
        }

        private void NextCardButton_Click(object sender, RoutedEventArgs e)
        {
            NextCard();
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Hard, (Button)sender);
            SaveDifficultySelection(GetCurrentStudyCard(), Card.CardResult.Hard);
            _showSavedDifficultySelection = true;
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Normal, (Button)sender);
            SaveDifficultySelection(GetCurrentStudyCard(), Card.CardResult.Normal);
            _showSavedDifficultySelection = true;
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Easy, (Button)sender);
            SaveDifficultySelection(GetCurrentStudyCard(), Card.CardResult.Easy);
            _showSavedDifficultySelection = true;
        }

        // Non-Button Methods
        private void NextCard()
        {
            if (_studySessionCards == null || _studySessionCards.Count == 0)
                return;
            _showSavedDifficultySelection = false;
            currentCardsInDeckPosition++;
            totalCardsShownCount++;
            Card currentCard = GetCurrentStudyCard();
            if (currentCard != null)
            { currentCard.LastResult = null; }
            if (StudyDeck.CurrentCardIndex >= _studySessionCards.Count - 1)
            {
                StartStudySession();
                UpdateCardScreen();
                return;
            }
            StudyDeck.NextCard();
            UpdateCardScreen();
        }

        private void UpdateCardScreen()
        {
            Card currentCard = GetCurrentStudyCard();
            if (currentCard == null) { return; }

            switch (currentCard.Difficulty)
            {
                case Card.CardResult.Hard:
                    CardScreen.DifficultyOfCard.Foreground = (Brush)FindResource("HardBrush");
                    CardScreen.DifficultyOfCard.Text = "Hard";
                    break;
                case Card.CardResult.Normal:
                    CardScreen.DifficultyOfCard.Foreground = (Brush)FindResource("NormalBrush");
                    CardScreen.DifficultyOfCard.Text = "Normal";
                    break;
                case Card.CardResult.Easy:
                    CardScreen.DifficultyOfCard.Foreground = (Brush)FindResource("EasyBrush");
                    CardScreen.DifficultyOfCard.Text = "Easy";
                    break;
                default:
                    CardScreen.DifficultyOfCard.Foreground = (Brush)FindResource("DefaultTextBrush");
                    CardScreen.DifficultyOfCard.Text = "None";
                    break;
            }

            CardScreen.MainCardViewControl.SetCard(currentCard);
            CardScreen.MainCardViewControl.SetAnswerVisible(false);//Toggles the AnswerText on/off

            CardScreen.ShowAnswerButtonControl.Visibility = Visibility.Visible;
            CardScreen.ShowAnswerButtonControl.IsEnabled = false;
            CardScreen.ShowAnswerButtonControl.Opacity = 0.4;

            CardScreen.NextCardButtonControl.Visibility = Visibility.Collapsed;
            CardScreen.NextCardButtonControl.IsEnabled = false;
            CardScreen.NextCardButtonControl.Opacity = 0.4;
            SelectDifficultyButton();
            CardScreen.CardsShownTextBlock.Text = $"Card: {currentCardsInDeckPosition} of {currentDeckCardsCount}";
            CardScreen.TotalCardsShown.Text = $"Total: {totalCardsShownCount}";
            if (_showSavedDifficultySelection && currentCard.LastResult.HasValue)
            {
                switch (currentCard.LastResult.Value)
                {
                    case Card.CardResult.Hard:
                        SelectButton(CardScreen.HardButtonControl);
                        break;

                    case Card.CardResult.Normal:
                        SelectButton(CardScreen.NormalButtonControl);
                        break;

                    case Card.CardResult.Easy:
                        SelectButton(CardScreen.EasyButtonControl);
                        break;
                }
            }
        }

        private int GetStudyPriority(Card card)
        {
            switch (card.Difficulty)
            {
                case Card.CardResult.Hard: return 0;
                case Card.CardResult.Normal: return 1;
                case Card.CardResult.Easy: return 2;
                default: return 3;
            }
            
        }

        private void SaveDifficultySelection(Card card, Card.CardResult diff)
        {
            if (card == null) { return; }
            card.Difficulty = diff;
            _IOLogic.SaveAllDecks(MainDecks);
        }

        private void HandleDifficultySelection(Card.CardResult result, Button button)
        {
            bool selected = ToggleCurrentCardResult(result);

            // --- State: No difficulty selected ---
            if (!selected)
            {
                // Toggles all Diff Buttons to 1.0 Opacity, Visible, !selected
                ResetDifficultyButtons();

                // ?
                SelectDifficultyButton();

                // Toggles AnswerButton off
                ShowAnswerButton(false);
                return;
            }

            // --- State: Difficulty selected ---
            
            // Toggles AnswerButton on
            ShowAnswerButton(true);
            
            // Toggle all buttons except selectedButton to 0.85 opacity, and IsEnabled=false
            SelectButton(button);
        }

        private void SelectDifficultyButton()
        {
            foreach (var btn in CardScreen.DifficultyButtons)
            {
                btn.BorderThickness = new Thickness(1);
                btn.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#555555");
                btn.Opacity = 1.0;
            }
        }

        private void ResetDifficultyButtons()
        {
            foreach (Button diffButton in CardScreen.DifficultyButtons)
            {
                diffButton.Opacity = 1.0;
            }
        }

        private void SelectButton(Button selected)
        {
            SelectDifficultyButton();
            selected.BorderThickness = new Thickness(2);
            selected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#C6C6C6");
            selected.Opacity = 1.0;
            foreach (var btn in CardScreen.DifficultyButtons)
            {
                if (btn != selected)
                    btn.Opacity = 0.85;
            }
        }

        public bool ToggleCurrentCardResult(Card.CardResult result)
        {
            Card currentCard = GetCurrentStudyCard();
            if (currentCard == null)
                return false;
            if (currentCard.LastResult == result)
            {
                currentCard.LastResult = null;
                return false;
            }
            currentCard.LastResult = result;
            return true;
        }

        private Card GetCurrentStudyCard()
        {
            if (_studySessionCards == null || _studySessionCards.Count == 0)
                return null;
            if (StudyDeck.CurrentCardIndex < 0 || StudyDeck.CurrentCardIndex >= _studySessionCards.Count)
                return null;
            return _studySessionCards[StudyDeck.CurrentCardIndex];
        }

        private void ShowAnswerButton(bool value)
        {
            switch (value)
            {
                case true:
                    CardScreen.ShowAnswerButtonControl.Opacity = 1.0;
                    CardScreen.ShowAnswerButtonControl.IsEnabled = true;
                    break;
                case false:
                    CardScreen.ShowAnswerButtonControl.Opacity = 0.4;
                    CardScreen.ShowAnswerButtonControl.IsEnabled = false;
                    break;
            }
        }
    }
}
