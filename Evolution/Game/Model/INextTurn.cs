using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    public interface INextTurn
    {
        void NextTurn(bool isNormalTurn);
    }
}
