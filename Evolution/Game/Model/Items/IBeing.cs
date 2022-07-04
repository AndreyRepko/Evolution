using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model
{
    public interface IBeing : INotifyPropertyChanged
    {
        Position WeakPosition { get; }

        BeingType Type { get; }
    }
}
