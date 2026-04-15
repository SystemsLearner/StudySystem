using StudySystem.Core.JCard;
using System;
using System.Windows;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void CreateTemplateDeck_Click(object sender, RoutedEventArgs e)
        {
            Deck templateDeck = new Deck();
            templateDeck.Name = "Template Deck";

            templateDeck.Cards.Add(new Card
            {
                Front = "食べる",
                Reading = "たべる",
                Pronunciation = "",
                Answer = "to eat",
                Difficulty = Card.CardResult.Unsure
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "飲む",
                Reading = "のむ",
                Pronunciation = "",
                Answer = "to drink",
                Difficulty = Card.CardResult.Unsure
            });

            templateDeck.Cards.Add(new Card
            {
                Front = "見る",
                Reading = "みる",
                Pronunciation = "",
                Answer = "to see",
                Difficulty = Card.CardResult.Unsure
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
    }
}
