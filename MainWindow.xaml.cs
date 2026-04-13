using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StudySystem.Core;
using StudySystem.Core.JCard;

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
            //HomeButtonControl
            InitializeComponent();
            LoadDecksFromDisk();
            EditorLoadDecksIntoComboBox();

            StudyScreen.DeckSelectionComboBoxControl.ItemsSource = _decks;
            StudyScreen.DeckSelectionComboBoxControl.DisplayMemberPath = "Name";

            BuilderScreen.FrontTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.ReadingTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.PronunciationTextBoxControl.TextChanged += BuilderFields_TextChanged;
            BuilderScreen.AnswerTextBoxControl.TextChanged += BuilderFields_TextChanged;

            BuilderScreen.DeckComboBoxControl.SelectionChanged += BuilderDeckComboBox_SelectionChanged;
            BuilderScreen.CardComboBoxControl.SelectionChanged += BuilderCardComboBox_SelectionChanged;
            StudyScreen.DeckSelectionComboBoxControl.SelectionChanged += StudyDeckComboBox_SelectionChanged;

            BuilderScreen.SaveButtonControl.Click += SaveDeckButton_Click;
            BuilderScreen.BackButtonControl.Click += BackToHomeButton_Click;
            //BuilderScreen.PreviousCardButtonControl.Click += PreviousCard_Click;
            //BuilderScreen.NextCardButtonControl.Click += NextCard_Click;
            //BuilderScreen.AddCardButtonControl.Click += AddCard_Click;
            //BuilderScreen.DeleteCardButtonControl.Click += DeleteCard_Click;

            HomeScreen.StudyButtonControl.Click += StudyButton_Click;
            HomeScreen.BuilderButtonControl.Click += BuilderButton_Click;
            HomeScreen.SettingsButtonControl.Click += SettingsButton_Click;
            HomeScreen.ExitButtonControl.Click += ExitButton_Click;
            HomeScreen.TemplateDeckControl.Click += CreateTemplateDeck_Click;

            SettingsScreen.HomeButtonControl.Click += BackToHomeButton_Click;

            StudyScreen.ImportDeckButtonControl.Click += ImportDeckButton_Click;
            StudyScreen.StudyButtonControl.Click += DeckStudyButton_Click;
            StudyScreen.BackButtonControl.Click += BackToHomeButton_Click;

            CardScreen.HardButtonControl.Click += HardButton_Click;
            CardScreen.NormalButtonControl.Click += NormalButton_Click;
            CardScreen.EasyButtonControl.Click += EasyButton_Click;
            CardScreen.ShowAnswerButtonControl.Click += ShowAnswerButton_Click;
            CardScreen.NextCardButtonControl.Click += NextCardButton_Click;
            CardScreen.BackButtonControl.Click += BackToHomeButton_Click;
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

        private void StudyDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deck selectedDeck = StudyScreen.DeckSelectionComboBoxControl.SelectedItem as Deck;
        }
    }
}
