using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    public interface IBeing : INextTurn
    {
        PositionReadOnly Position { get; }
    }
}
