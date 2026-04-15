using System.Windows;
using System.Windows.Controls;
using StudySystem.Core.JCard;
using StudySystem.Screens;

namespace StudySystem.Controls
{
    public partial class CardView : UserControl
    {
        private Card _currentCard;

        public CardView()
        {
            InitializeComponent();
        }

        public void UpdateCard(Card card)
        {
            card.Front = CardFrontText.Text;
            card.Reading = CardReadingText.Text;
            card.Extras = CardExtrasText.Text;
            card.Pronunciation = CardPronunciationText.Text;
            card.Answer = CardAnswerText.Text;
        }

        public string CardTitle
        {
            get { return (string)GetValue(CardTitleProperty); }
            set { SetValue(CardTitleProperty, value); }
        }

        public static readonly DependencyProperty CardTitleProperty =
            DependencyProperty.Register(
                nameof(CardTitle),
                typeof(string),
                typeof(CardView),
                new PropertyMetadata(string.Empty));

        public string ReadingText
        {
            get { return (string)GetValue(ReadingTextProperty); }
            set { SetValue(ReadingTextProperty, value); }
        }

        public static readonly DependencyProperty ReadingTextProperty =
            DependencyProperty.Register(
                nameof(ReadingText),
                typeof(string),
                typeof(CardView),
                new PropertyMetadata(string.Empty));

        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }

        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register(
                nameof(MainText),
                typeof(string),
                typeof(CardView),
                new PropertyMetadata(string.Empty));

        public string BottomText
        {
            get { return (string)GetValue(BottomTextProperty); }
            set { SetValue(BottomTextProperty, value); }
        }

        public void SetCard(Card card)
        {
            _currentCard = card;
            CardFrontText.Text = card.Front;
            CardReadingText.Text = card.Reading;
            CardExtrasText.Text = card.Extras;
            CardPronunciationText.Text = card.Pronunciation;
            CardAnswerText.Text = card.Answer;
        }

        public static readonly DependencyProperty BottomTextProperty =
            DependencyProperty.Register(
                nameof(BottomText),
                typeof(string),
                typeof(CardView),
                new PropertyMetadata(string.Empty));
        public void SetCardValues(string front, string reading,
            string answer, string pronunciation, string extrasText)
        {
            CardFrontText.Text = front;
            CardReadingText.Text = reading;
            CardExtrasText.Text = extrasText;
            CardAnswerText.Text = answer;
            CardPronunciationText.Text = pronunciation;
            CardExtrasText.Text = extrasText;
        }

        public void SetAnswerVisible(bool visible)
        {
            CardAnswerText.Visibility = visible
                ? Visibility.Visible
                : Visibility.Hidden;
        }
    }
}
