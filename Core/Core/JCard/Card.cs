using Newtonsoft.Json;
using System.ComponentModel;
using static StudySystem.Core.JCard.Card;

namespace StudySystem.Core.JCard
{
    public class Card : INotifyPropertyChanged
    {
        private string _front;
        private string _reading;
        private string _extras;
        private string _pronunciation;
        private string _answer;
        private CardResult _difficulty;
        private CardResult? _lastResult;
        private int _index;
        public string Front
        {
            get => _front;
            set
            {
                if (_front == value) return;
                _front = value;
                OnPropertyChanged(nameof(Front));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        public string Reading
        {
            get => _reading;
            set
            {
                if (_reading == value) return;
                _reading = value;
                OnPropertyChanged(nameof(Reading));
            }
        }
        public string Extras
        {
            get => _extras;
            set
            {
                if (_extras == value) return;
                _extras = value;
                OnPropertyChanged(nameof(Extras));
            }
        }
        public string Pronunciation
        {
            get => _pronunciation;
            set
            {
                if (_pronunciation == value) return;
                _pronunciation = value;
                OnPropertyChanged(nameof(Pronunciation));
            }
        }
        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer == value) return;
                _answer = value;
                OnPropertyChanged(nameof(Answer));
            }
        }
        public CardResult Difficulty
        {
            get => _difficulty;
            set
            {
                if (_difficulty == value) return;
                _difficulty = value;
                OnPropertyChanged(nameof(Difficulty));
            }
        }
        [JsonIgnore]
        public CardResult? LastResult
        {
            get => _lastResult;
            set
            {
                if (_lastResult == value) return;
                _lastResult = value;
                OnPropertyChanged(nameof(LastResult));
            }
        }
        [JsonIgnore]
        public int Index
        {
            get => _index;
            set
            {
                if (_index == value) return;
                _index = value;
                OnPropertyChanged(nameof(Index));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Front))
                    return $"[{Index}] [Empty Card]";
                return $"[{Index}] {Front}";
            }
        }
        public enum CardResult
        {
            Hard = 0,
            Normal = 1,
            Easy = 2
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
