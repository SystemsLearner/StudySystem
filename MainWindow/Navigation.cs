using System.Windows;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void ShowScreen(UIElement screen)
        {
            foreach (UIElement child in MainGrid.Children)
            {
                child.Visibility = Visibility.Collapsed;
                child.IsEnabled = false;
            }
            screen.Visibility = Visibility.Visible;
            screen.IsEnabled = true;
        }

        private void StudyButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(StudyScreen);
        }

        private void BuilderButton_Click(object sender, RoutedEventArgs e)
        {
            BuilderScreen.EditorCardViewControl.CardAnswerText.Visibility = Visibility.Visible;
            ShowScreen(BuilderScreen);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(SettingsScreen);
        }

        private void BackToHomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(HomeScreen);
            totalCardsShownCount = 0;
        }

        private void BackToStudyButton_Click(object sender, RoutedEventArgs e)
        {
            currentCardsInDeckPosition = 1;
            ShowScreen(StudyScreen);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
