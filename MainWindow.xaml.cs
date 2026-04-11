using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudySystem.Core;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private List<Deck> _decks = new List<Deck>();
        private Deck _currentDeck;
        private int _currentCardIndex = 0;
        private bool _difficultySelected = false;
        public MainWindow()
        {
            InitializeComponent();

            LoadTestDecks();
            DeckSelection.ItemsSource = _decks;
            DeckSelection.DisplayMemberPath = "Name";

        }

        private void LoadTestDecks()
        {
            Deck TestDeck = new Deck();
            TestDeck.Name = "Manually Loaded Test Deck";

            TestDeck.Cards.Add(new Card
            {
                Front = "猫",
                Furigana = "ねこ",
                Intonation = "neko",
                Answer = "Cat",
                ShowFuriganaByDefault = true
            });

            TestDeck.Cards.Add(new Card
            {
                Front = "犬",
                Furigana = "いぬ",
                Intonation = "inu",
                Answer = "Dog",
                ShowFuriganaByDefault = true
            });

            _decks.Add(TestDeck);

            Deck TestDeck2 = new Deck();
            TestDeck2.Name = "Manually Loaded Test Deck 2";

            TestDeck2.Cards.Add(new Card
            {
                Front = "食べる",
                Furigana = "たべる",
                Intonation = "taberu",
                Answer = "To Eat",
                ShowFuriganaByDefault = true
            });

            _decks.Add(TestDeck2);
        }

        private void ShowScreen(UIElement screen)
        {
            foreach (UIElement child in MainGrid.Children)
            {
                if (child is Grid)
                {
                    child.Visibility = Visibility.Collapsed;
                }
            }

            screen.Visibility = Visibility.Visible;
        }

        private void StudyButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(StudyScreen);
        }

        private void DeckStudyButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = DeckSelection.SelectedItem as Deck;
            if (selectedDeck == null)
            {
                return;
            }

            if (selectedDeck == null || selectedDeck.Cards.Count == 0)
            {
                return;
            }

            _currentDeck = selectedDeck;
            _currentCardIndex = 0;

            UpdateCardScreen();
            ShowScreen(CardScreen);
        }

        private void UpdateCardScreen()
        {
            if (_currentDeck == null || _currentDeck.Cards.Count == 0)
            {
                return;
            }

            Card currentCard = _currentDeck.Cards[_currentCardIndex];
            CardFrontText.Text = currentCard.Front;
            CardFuriganaText.Text = currentCard.FuriganaDisplay;
            CardAnswerText.Text = currentCard.Answer;
            CardIntonationText.Text = currentCard.Intonation;
            HardButton.IsEnabled = true;
            NormalButton.IsEnabled = true;
            EasyButton.IsEnabled = true;
            _difficultySelected = false;
            AnswerContainer.Visibility = Visibility.Collapsed;
            ShowAnswerButton.Visibility = Visibility.Visible;
            ShowAnswerButton.IsEnabled = false;
            ShowAnswerButton.Opacity = 0.4;
            NextButton.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;
            NextButton.Opacity = 0.4;
            ResetButtons();
        }

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerContainer.Visibility = Visibility.Visible;
            ShowAnswerButton.Visibility = Visibility.Collapsed;
            ShowAnswerButton.IsEnabled = false;
            NextButton.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;
            NextButton.Opacity = 1.0;
        }

        private void NextCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDeck == null || _currentDeck.Cards.Count == 0)
            {
                return;
            }
            _currentCardIndex++;
            if (_currentCardIndex >= _currentDeck.Cards.Count)
            {
                _currentCardIndex = 0;
            }
            UpdateCardScreen();
        }

        private void SelectButton(Button selected)
        {
            ResetButtons();
            selected.BorderThickness = new Thickness(2);
            selected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#C6C6C6");
            selected.Opacity = 1.0;

            foreach (var btn in new[] { HardButton, NormalButton, EasyButton })
            {
                if (btn != selected)
                    btn.Opacity = 0.85;
            }
        }

        private void ResetButtons()
        {
            foreach (var btn in new[] { HardButton, NormalButton, EasyButton })
            {
                btn.BorderThickness = new Thickness(1);
                btn.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#555555");
                btn.Opacity = 1.0;
            }
        }

        private void EnableAnswer()
        {
            ShowAnswerButton.IsEnabled = true;
            ShowAnswerButton.Opacity = 1.0;
        }

        private void SettingsButton_Click(Object sender, RoutedEventArgs e)
        {
            ShowScreen(SettingsScreen);
        }

        private void BackToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(HomeScreen);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            _difficultySelected = true;
            _currentDeck.Cards[_currentCardIndex].LastResult = Card.CardResult.Hard;
            SelectButton(HardButton);
            EnableAnswer();
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            _difficultySelected = true;
            _currentDeck.Cards[_currentCardIndex].LastResult = Card.CardResult.Normal;
            SelectButton(NormalButton);
            EnableAnswer();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            _difficultySelected = true;
            _currentDeck.Cards[_currentCardIndex].LastResult = Card.CardResult.Easy;
            SelectButton(EasyButton);
            EnableAnswer();
        }
    }
}
