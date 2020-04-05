using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    public interface IBeing
    {
        PositionReadOnly Position { get; }

        BeingType Type { get; }
    }
}
