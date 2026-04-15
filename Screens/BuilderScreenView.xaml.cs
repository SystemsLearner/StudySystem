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
        public Button SaveDeckButtonControl => SaveDeck;
        public Button PreviousCardButtonControl => PreviousCard;
        public Button NextCardButtonControl => NextCard;
        public Button AddCardButtonControl => AddCard;
        public Button DeleteCardButtonControl => DeleteCard;
        public ComboBox DeckComboBoxControl => DeckComboBox;
        public ComboBox CardComboBoxControl => CardComboBox;
        public CardView EditorCardViewControl => EditorCardView;
        public BuilderPanel FrontFieldControl => FrontField;
        public BuilderPanel ReadingFieldControl => ReadingField;
        public BuilderPanel ExtrasFieldControl => ExtrasField;
        public BuilderPanel PronunciationFieldControl => PronunciationField;
        public BuilderPanel AnswerFieldControl => AnswerField;
    }
}
