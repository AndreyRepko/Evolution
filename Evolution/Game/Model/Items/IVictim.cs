using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model.Items
{
    public interface IVictim
    {
        void NotifyAboutAggressionChange(bool aggression);
    }
}
