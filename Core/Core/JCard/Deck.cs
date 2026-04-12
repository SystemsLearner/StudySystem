using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Core.JCard
{
    public class Deck
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return "[Unnamed Deck]";
                return Name;
            }
        }
    }
}
