using System.Windows.Controls;
using StudySystem.Controls;

namespace StudySystem.Screens
{
    public partial class HomeScreenView : UserControl
    {
        public HomeScreenView()
        {
            InitializeComponent();
        }

        public Button StudyButtonControl => StudyButton;
        public Button BuilderButtonControl => BuilderButton;
        public Button SettingsButtonControl => SettingsButton;
        public Button ExitButtonControl => ExitButton;
        public Button TemplateDeckControl => TemplateButton;
    }
}
