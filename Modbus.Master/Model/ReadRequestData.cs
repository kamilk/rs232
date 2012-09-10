﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialPortCommunicator.Modbus.Master.Model
{
    struct ReadRequestData
    {
        public Action<short> OnSuccessCallback;
        public byte Address;
        public short Register;
    }
}