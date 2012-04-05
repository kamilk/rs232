using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS232.Parameters
{
    public enum XONType
    {
        PROGRAM, RTSCTS, DTRDSR
    }

    public class XONTypeMenuItem
    {
        public XONType type { get; private set; }
        public XONTypeMenuItem(XONType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            switch (type)
            {
                case XONType.PROGRAM:
                    return "CTRL-S/CTRL-Q";
                case XONType.DTRDSR:
                    return "DTR/DSR";
                case XONType.RTSCTS:
                    return "RTS/CTR";
                default:
                    return "Błędny typ";
            }
        }
    }
}
