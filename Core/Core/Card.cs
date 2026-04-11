using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Core
{
    public class Card
    {
        public string Front {  get; set; }
        public string Furigana { get; set; }
        public string Answer { get; set; }
        public string Intonation { get; set; }
        public CardResult LastResult { get; set; }
        public bool ShowFuriganaByDefault { get; set; }

        public string FuriganaDisplay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Furigana))
                {
                    return "";
                }
                return $"({Furigana})";
            }
        }

        public enum CardResult
        {
            Hard = 0,
            Normal = 1,
            Easy = 2
        }
    }
}
