using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Core
{
    public class Deck
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
