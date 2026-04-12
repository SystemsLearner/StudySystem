using StudySystem.Controls;
using System.Windows.Controls;
using System.Collections.Generic;

namespace StudySystem.Screens
{
    public partial class CardScreenView : UserControl
    {
        public CardScreenView()
        {
            InitializeComponent();
        }

        public Button HardButtonControl => HardButton;
        public Button NormalButtonControl => NormalButton;
        public Button EasyButtonControl => EasyButton;
        public Button ShowAnswerButtonControl => ShowAnswerButton;
        public Button NextCardButtonControl => NextCardButton;
        public Button BackButtonControl => BackButton;
        public CardView MainCardViewControl => MainCardView;
        public IEnumerable<Button> DifficultyButtons =>
            new[] { HardButton, NormalButton, EasyButton };
    }
}
