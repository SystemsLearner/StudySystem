using StudySystem.Core.JCard;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void UpdateCardScreen()
        {

            Card currentCard = _logic.CurrentCard;
            if (currentCard == null) { return; }

            CardScreen.MainCardViewControl.SetCard(currentCard);
            CardScreen.MainCardViewControl.SetAnswerVisible(false);//Toggles the AnswerText on/off
            currentCard.LastResult = null;

            CardScreen.ShowAnswerButtonControl.Visibility = Visibility.Visible;
            CardScreen.ShowAnswerButtonControl.IsEnabled = false;
            CardScreen.ShowAnswerButtonControl.Opacity = 0.4;

            CardScreen.NextCardButtonControl.Visibility = Visibility.Collapsed;
            CardScreen.NextCardButtonControl.IsEnabled = false;
            CardScreen.NextCardButtonControl.Opacity = 0.4;
            SelectDifficultyButton();
            if (currentCard.LastResult.HasValue)
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

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            CardScreen.MainCardViewControl.SetAnswerVisible(true);//Toggles the AnswerText on/off
            CardScreen.ShowAnswerButtonControl.Visibility = Visibility.Collapsed;
            CardScreen.NextCardButtonControl.Visibility = Visibility.Visible;
            CardScreen.NextCardButtonControl.IsEnabled = true;
            CardScreen.NextCardButtonControl.Opacity = 1.0;
        }

        private void NextCardButton_Click(object sender, RoutedEventArgs e)
        {
            _logic.NextCard();
            UpdateCardScreen();
        }

        private void HandleDifficultySelection(Card.CardResult result, Button button)
        {
            bool selected = _logic.ToggleResult(result);

            // --- State: No difficulty selected ---
            if (!selected)
            {
                ResetDifficultyButtons();
                SelectDifficultyButton();
                ShowAnswerButton(false);// Toggles AnswerButton on/off
                return;
            }

            // --- State: Difficulty selected ---
            ShowAnswerButton(true);// Toggles AnswerButton on/off
            SelectButton(button);
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Hard, (Button)sender);
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Normal, (Button)sender);
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDifficultySelection(Card.CardResult.Easy, (Button)sender);
        }

        private void ResetDifficultyButtons()
        {
            CardScreen.EasyButtonControl.Opacity = 1.0;
            CardScreen.NormalButtonControl.Opacity = 1.0;
            CardScreen.HardButtonControl.Opacity = 1.0;
            foreach (Button diffButton in CardScreen.DifficultyButtons)
            {
                diffButton.Opacity = 1.0;
            }
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
