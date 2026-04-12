using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StudySystem.Core;
using StudySystem.Core.JCard;
using StudySystem.Controls;
using StudySystem.Screens;
using System.Linq;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private List<Deck> _decks = new List<Deck>();
        private StudySession _logic = new StudySession();
        private DeckIO _IOLogic = new DeckIO();
        //private List<UIElement> _screens;
        private Deck SelectedEditorDeck;
        private Card SelectedEditorCard;
        public MainWindow()
        {
            InitializeComponent();
            LoadDecksFromDisk();
            EditorLoadDecksIntoComboBox();

            DeckSelection.ItemsSource = _decks;
            DeckSelection.DisplayMemberPath = "Name";

            BuilderScreen.FrontTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.ReadingTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.PronunciationTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.AnswerTextBoxControl.TextChanged += BuilderFields_TextChanged;

            BuilderScreen.DeckComboBoxControl.SelectionChanged += DeckComboBox_SelectionChanged;
            BuilderScreen.CardComboBoxControl.SelectionChanged += CardComboBox_SelectionChanged;

            BuilderScreen.HomeButtonControl.Click += BackToHomeButton_Click;
            BuilderScreen.SaveButtonControl.Click += SaveDeckButton_Click;
            //BuilderScreen.PreviousCardButtonControl.Click += PreviousCard_Click;
            //BuilderScreen.NextCardButtonControl.Click += NextCard_Click;
            //BuilderScreen.AddCardButtonControl.Click += AddCard_Click;
            //BuilderScreen.DeleteCardButtonControl.Click += DeleteCard_Click;
            HomeScreen.StudyButtonControl.Click += StudyButton_Click;
            HomeScreen.BuilderButtonControl.Click += BuilderButton_Click;
            HomeScreen.SettingsButtonControl.Click += SettingsButton_Click;
            HomeScreen.ExitButtonControl.Click += ExitButton_Click;
            HomeScreen.TemplateDeckControl.Click += CreateTemplateDeck_Click;

        }

        private void ShowScreen(UIElement screen)
        {

            //HomeScreen.Visibility = Visibility.Collapsed;
            //StudyScreen.Visibility = Visibility.Collapsed;
            //BuilderScreen.Visibility = Visibility.Collapsed;
            //SettingsScreen.Visibility = Visibility.Collapsed;
            //CardScreen.Visibility = Visibility.Collapsed;
            //
            //screen.Visibility = Visibility.Visible;

            foreach (UIElement child in MainGrid.Children)
            {
                child.Visibility = Visibility.Collapsed;
                child.IsEnabled = false;
            }
            screen.Visibility = Visibility.Visible;
            screen.IsEnabled = true;
        }

        //private void InitializeScreens()
        //{
        //    _screens = new List<UIElement>
        //    {
        //        HomeScreen,
        //        StudyScreen,
        //        BuilderScreen,
        //        SettingsScreen,
        //        CardScreen
        //    };
        //}

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
            BuilderScreen.EditorCardViewControl.CardAnswerText.Visibility = Visibility.Visible;
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
            RefreshEditorDeckSelection();
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
            RefreshEditorDeckSelection();
        }

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
                SelectedEditorCard.Front = BuilderScreen.EditorCardViewControl.CardFrontText.Text;
                SelectedEditorCard.Reading = BuilderScreen.EditorCardViewControl.CardReadingText.Text;
                SelectedEditorCard.Answer = BuilderScreen.EditorCardViewControl.CardAnswerText.Text;
                SelectedEditorCard.Pronunciation = BuilderScreen.EditorCardViewControl.CardPronunciationText.Text;
            }
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
            RefreshStudyDeckSelection();
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

        private void EditorLoadDecksIntoComboBox()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = _decks;
        }

        private void RefreshStudyDeckSelection()
        {
            DeckSelection.ItemsSource = null;
            DeckSelection.ItemsSource = _decks;
            DeckSelection.DisplayMemberPath = "Name";
        }

        private void RefreshEditorDeckSelection()
        {
            BuilderScreen.DeckComboBoxControl.ItemsSource = null;
            BuilderScreen.DeckComboBoxControl.ItemsSource = _decks;
            BuilderScreen.DeckComboBoxControl.DisplayMemberPath = "DisplayName";
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

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void DeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void CardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card selectedCard = BuilderScreen.CardComboBoxControl.SelectedItem as Card;

            if (selectedCard == null)
            {
                BuilderScreen.FrontTextBoxControl.Text = "";
                BuilderScreen.ReadingTextBoxControl.Text = "";
                BuilderScreen.PronunciationTextBoxControl.Text = "";
                BuilderScreen.AnswerTextBoxControl.Text = "";
                RefreshBuilderPreview();
                return;
            }

            BuilderScreen.FrontTextBoxControl.Text = selectedCard.Front;
            BuilderScreen.ReadingTextBoxControl.Text = selectedCard.Reading;
            BuilderScreen.PronunciationTextBoxControl.Text = selectedCard.Pronunciation;
            BuilderScreen.AnswerTextBoxControl.Text = selectedCard.Answer;

            RefreshBuilderPreview();
        }
    }
}
