using StudySystem.Controls;
using System.Windows.Controls;

namespace StudySystem.Screens
{
    public partial class BuilderScreenView : UserControl
    {
        public BuilderScreenView()
        {
            InitializeComponent();
        }
        public Button BackButtonControl => BackButton;
        public Button SaveButtonControl => Save;
        public Button PreviousCardButtonControl => PreviousCard;
        public Button NextCardButtonControl => NextCard;
        public Button AddCardButtonControl => AddCard;
        public Button DeleteCardButtonControl => DeleteCard;
        public TextBox FrontTextBoxControl => BuilderFrontTextBox;
        public TextBox ReadingTextBoxControl => BuilderReadingTextBox;
        public TextBox PronunciationTextBoxControl => BuilderPronunciationTextBox;
        public TextBox AnswerTextBoxControl => BuilderAnswerTextBox;
        public ComboBox DeckComboBoxControl => DeckComboBox;
        public ComboBox CardComboBoxControl => CardComboBox;
        public CardView EditorCardViewControl => EditorCardView;
    }
}
