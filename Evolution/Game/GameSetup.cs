using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game
{
    public class GameSetup
    {
        public int BoardSize { get; set; } = 50;
        public int InitialZavrsCount { get; set; } = 40;
        public int InitialTreesCount { get; set; } = 0;
        public int InitialEnergyBoxCount { get; set; } = 50;
        public int EnergyBoxNutrition { get; set; } = 50;
    }
}
