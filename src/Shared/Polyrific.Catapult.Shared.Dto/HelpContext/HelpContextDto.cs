using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Catapult.Shared.Dto.HelpContext
{
    public class HelpContextDto
    {
        public string Section { get; set; }

        public string SubSection { get; set; }

        public string Text { get; set; }

        public int Sequence { get; set; }
    }
}
