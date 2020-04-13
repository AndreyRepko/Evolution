using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    public interface IVegetetableWorldInteraction
    {
        void SpawnNewVegetable(Position position, Directions directions);
    }
}
