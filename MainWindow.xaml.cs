using Microsoft.Win32;
using StudySystem.Core;
using StudySystem.Core.JCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using StudySystem.Controls;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private List<Deck> _decks = new List<Deck>();
        private StudySession _logic = new StudySession();
        private DeckIO _IOLogic = new DeckIO();
        public MainWindow()
        {
            InitializeComponent();
            LoadDecksFromDisk();
            EditorLoadDecksFromDisk();
        }

        private void ShowScreen(UIElement screen)
        {
            foreach (UIElement child in MainGrid.Children)
            {
                if (child is Grid grid)
                {
                    grid.Visibility = Visibility.Collapsed;
                    grid.IsEnabled = false;
                }
            }

            if (screen is Grid targetGrid)
            {
                targetGrid.Visibility = Visibility.Visible;
                targetGrid.IsEnabled = true;
            }
        }

        private void StudyButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(StudyScreen);
        }

        private void DeckStudyButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = DeckSelection.SelectedItem as Deck;

            if (selectedDeck == null || selectedDeck.Cards.Count == 0) { return; }

            _logic.StartDeck(selectedDeck);

            UpdateCardScreen();
            ShowScreen(CardScreen);
        }

        private void UpdateCardScreen()
        {

            Card currentCard = _logic.CurrentCard;
            if (currentCard == null) { return; }

            MainCardView.CardFrontText.Text = currentCard.Front;
            MainCardView.CardReadingText.Text = currentCard.Reading;
            MainCardView.CardAnswerText.Text = currentCard.Answer;
            MainCardView.CardPronunciationText.Text = currentCard.Pronunciation;
            MainCardView.SetAnswerVisible(false);//Toggles the AnswerText on/off
            currentCard.LastResult = null;

            ShowAnswerButton.Visibility = Visibility.Visible;
            ShowAnswerButton.IsEnabled = false;
            ShowAnswerButton.Opacity = 0.4;

            NextButton.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;
            NextButton.Opacity = 0.4;
            SelectDifficultyButton();
            if (currentCard.LastResult.HasValue)
            {
                switch (currentCard.LastResult.Value)
                {
                    case Card.CardResult.Hard:
                        SelectButton(HardButton);
                        break;

                    case Card.CardResult.Normal:
                        SelectButton(NormalButton);
                        break;

                    case Card.CardResult.Easy:
                        SelectButton(EasyButton);
                        break;
                }
            }
        }

        private void BuilderFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshBuilderPreview();
        }

        private void RefreshBuilderPreview()
        {
            EditorCardView.SetCard(new Card
            {
                Front = BuilderFrontTextBox.Text,
                Reading = BuilderReadingTextBox.Text,
                Pronunciation = BuilderPronunciationTextBox.Text,
                Answer = BuilderAnswerTextBox.Text
            });
        }

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            MainCardView.SetAnswerVisible(true);//Toggles the AnswerText on/off
            ShowAnswerButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;
            NextButton.Opacity = 1.0;
        }

        private void NextCardButton_Click(object sender, RoutedEventArgs e)
        {
            _logic.NextCard();
            UpdateCardScreen();
        }

        private void BuilderButton_Click(object sender, RoutedEventArgs e)
        {
            EditorCardView.CardAnswerText.Visibility = Visibility.Visible;
            ShowScreen(BuilderScreen);
        }

        private void SelectButton(Button selected)
        {
            SelectDifficultyButton();
            selected.BorderThickness = new Thickness(2);
            selected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#C6C6C6");
            selected.Opacity = 1.0;

            foreach (var btn in new[] { HardButton, NormalButton, EasyButton })
            {
                if (btn != selected)
                    btn.Opacity = 0.85;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
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

        private void HandleDifficultySelection(Card.CardResult result, Button button)
        {
            bool selected = _logic.ToggleResult(result);

            if (!selected)
            {
                ResetDifficultyButtons();
                SelectDifficultyButton();
                _ShowAnswerButton(false);//Toggles AnswerButton on/off
                return;
            }
            _ShowAnswerButton(true);//Toggles AnswerButton on/off
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
            EasyButton.Opacity = 1.0;
            NormalButton.Opacity = 1.0;
            HardButton.Opacity = 1.0;
        }

        private void SelectDifficultyButton()
        {
            foreach (var btn in new[] { HardButton, NormalButton, EasyButton })
            {
                btn.BorderThickness = new Thickness(1);
                btn.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#555555");
                btn.Opacity = 1.0;
            }
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
                    //already exists
                    MessageBox.Show("A deck with this name already exists." +
                        "\r\n\r\nPlease choose a different name or replace the existing file.");
                }
                else if (!File.Exists(destPath))
                {
                    File.Copy(sourcePath, destPath, true);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error has occured: \n" + ex.Message);
            }

            LoadDecksFromDisk();
            RefreshDeckSelection();
        }

        private void ImportDecksButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JCard Files (*.jcard)|*.jcard",
                Multiselect = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string destFolder = _IOLogic.GetDecksFolder();

            foreach (string sourcePath in dialog.FileNames)
            {
                string fileName = System.IO.Path.GetFileName(sourcePath);
                string destPath = System.IO.Path.Combine(destFolder, fileName);

                File.Copy(sourcePath, destPath, true);
            }
            MessageBox.Show("Deck(s) imported successfully.");
            LoadDecksFromDisk();
            RefreshDeckSelection();
        }

        private void SaveDeckButton_Click(object sender, RoutedEventArgs e)
        {
            Deck selectedDeck = DeckSelection.SelectedItem as Deck;

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

            MessageBox.Show("Deck saved.");
        }

        private void CreateTemplateDeck_Click(object sender, RoutedEventArgs e)
        {
            Deck templateDeck = new Deck();
            templateDeck.Name = "Template Deck";

            templateDeck.Cards.Add(new Card
            {
                Front = "食べる",
                Reading = "たべる",
                Pronunciation = "",
                Answer = "to eat"
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "飲む",
                Reading = "のむ",
                Pronunciation = "",
                Answer = "to drink"
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "見る",
                Reading = "みる",
                Pronunciation = "",
                Answer = "to see"
            });

            string folder = _IOLogic.GetDecksFolder();
            string fileName = templateDeck.Name.Replace(" ", "") + ".jcard";
            fileName = fileName.Replace("_", "");
            string path = System.IO.Path.Combine(folder, fileName);
            try
            {
                _IOLogic.WriteDeck(templateDeck, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error happened: \n" + ex.Message);
            }

            LoadDecksFromDisk();
            RefreshDeckSelection();
        }

        private void _ShowAnswerButton(bool value)
        {
            switch (value)
            {
                case true:
                    ShowAnswerButton.Opacity = 1.0;
                    ShowAnswerButton.IsEnabled = true;
                    break;
                case false:
                    ShowAnswerButton.Opacity = 0.4;
                    ShowAnswerButton.IsEnabled = false;
                    break;
            }
        }

        private void SaveAllDecksButton_Click(object sender, RoutedEventArgs e)
        {
            _IOLogic.SaveAllDecks(_decks);
        }

        private void LoadDecksFromDisk()
        {
            _decks = _IOLogic.LoadAllDecks();
        }

        private void EditorLoadDecksFromDisk()
        {
            DeckComboBox.ItemsSource = _decks;
        }

        private void RefreshDeckSelection()
        {
            DeckSelection.ItemsSource = null;
            DeckSelection.ItemsSource = _decks;
            DeckSelection.DisplayMemberPath = "Name";
        }

        private void DeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deck selectedDeck = DeckComboBox.SelectedItem as Deck;

            if (selectedDeck == null)
            {
                CardComboBox.ItemsSource = null;
                return;
            }

            for (int i = 0; i < selectedDeck.Cards.Count; i++)
            {
                selectedDeck.Cards[i].Index = i + 1;
            }

            CardComboBox.ItemsSource = null;
            CardComboBox.ItemsSource = selectedDeck.Cards;
        }

        private void CardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card selectedCard = CardComboBox.SelectedItem as Card;

            if (selectedCard == null)
            {
                BuilderFrontTextBox.Text = "";
                BuilderReadingTextBox.Text = "";
                BuilderPronunciationTextBox.Text = "";
                BuilderAnswerTextBox.Text = "";
                RefreshBuilderPreview();
                return;
            }

            BuilderFrontTextBox.Text = selectedCard.Front;
            BuilderReadingTextBox.Text = selectedCard.Reading;
            BuilderPronunciationTextBox.Text = selectedCard.Pronunciation;
            BuilderAnswerTextBox.Text = selectedCard.Answer;

            RefreshBuilderPreview();
        }
    }
}
