using System.Windows;

namespace StudySystem
{
    public partial class MainWindow : Window
    {
        private void ShowScreen(UIElement screen)
        {

            //HomeScreen.Visibility = Visibility.Collapsed;
            //StudyScreen.Visibility = Visibility.Collapsed;
            //BuilderScreen.Visibility = Visibility.Collapsed;
            //SettingsScreen.Visibility = Visibility.Collapsed;
            //CardScreen.Visibility = Visibility.Collapsed;
            //screen.Visibility = Visibility.Visible;

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
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
