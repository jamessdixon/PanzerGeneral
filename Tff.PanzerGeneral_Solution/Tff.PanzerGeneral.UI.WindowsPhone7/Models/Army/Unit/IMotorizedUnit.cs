﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tff.Panzer.Models.Army.Unit
{
    public interface IMotorizedUnit: IUnit
    {
        int CurrentFuel { get; set; }
    }
}
