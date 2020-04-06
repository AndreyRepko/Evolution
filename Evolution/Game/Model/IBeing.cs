using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Evolution.Game.Model
{
    public interface IBeing : INotifyPropertyChanged
    {
        Position Position { get; }

        BeingType Type { get; }
    }
}
