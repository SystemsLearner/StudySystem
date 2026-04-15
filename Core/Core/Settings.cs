using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Core
{
    public class Settings
    {
        /*-- Furigana --*/
        public bool ShowReadingByDefault { get; set; } = true;
        public bool ShowExtrasByDefault { get; set; } = true;
        public bool AutoAdvanceAfterAnswer { get; set; } = false;
        public bool AutoPlayAudio { get; set; } = true;
    }
}