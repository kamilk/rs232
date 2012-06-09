using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Generic.Parameters
{
    public enum EndMarker
    {
        LF, CR, CRLF, NONE
    }

    public class EndMarkerMenuItem
    {
        public EndMarker type { get; private set; }
        public EndMarkerMenuItem(EndMarker type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            switch (type)
            {
                case EndMarker.LF:
                    return "LF";
                case EndMarker.CR:
                    return "CR";
                case EndMarker.CRLF:
                    return "Para CR,LF";
                case EndMarker.NONE:
                    return "Brak znacznika";
                default:
                    return "Błędny typ";
            }
        }
    }
}
