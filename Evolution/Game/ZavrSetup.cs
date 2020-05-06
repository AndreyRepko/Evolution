using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game
{
    public class ZavrSetup
    {
        public int InitialMaxAge {get; set;} = 100;
        public int InitialMaxEnergy { get; set; } = 5000;
        public int InitialStartEnergy { get; set; } = 3000;
        public int InitialExpendEnergyToRotate { get; set; } = 5;
        public int InitialExpendEnergyToReplicate { get; set; } = 2;
        public int InitialEnergyToEat { get; set; } = 5;
        public int InitialPowerDirection { get; set; } = 1;
        public int InitialCostOfMovementTwo { get; set; } = 10;
        public int InitialCostOfMovementOne { get; set; } = 2;
    }
}
