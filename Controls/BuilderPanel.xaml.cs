using System.Windows;
using System.Windows.Controls;

namespace StudySystem.Controls
{
    public partial class BuilderPanel : UserControl
    {
        public TextBox InputTextBoxControl => InputTextBox;
        public BuilderPanel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                nameof(LabelText),
                typeof(string),
                typeof(BuilderPanel),
                new PropertyMetadata(""));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register(
                nameof(InputText),
                typeof(string),
                typeof(BuilderPanel),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string InputText
        {
            get => (string)GetValue(InputTextProperty);
            set => SetValue(InputTextProperty, value);
        }
    }
}
