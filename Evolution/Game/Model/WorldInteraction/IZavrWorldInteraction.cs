using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Game.Model.Items;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model
{
    /// <summary>
    /// Information about what Zavr actually can see in the world
    /// Please, take attention that exact distance and if food is aggressive is not available at the moment
    /// </summary>
    public class SeenItems : List<(Directions where, int nutrition, object item)>
    { }

    public interface IZavrWorldInteraction
    {
        /// <summary>
        /// What the zavr can see.
        /// </summary>
        /// <param name="zavrPosition">The zavr position.</param>
        /// <param name="sight">The sight distance.</param>
        /// <param name="direction">Direction of view of zavr</param>
        /// <returns></returns>
        SeenItems WhatZavrCanSee(IBeing zavr, int sight, Directions direction);
        bool CanEat(IBeing zavr);
        int EatVegetable(IBeing zavr);
        void MarkItemAsVictim(object victim, object aggressor);
        void MarkZavrAsDead(Zavr zavr);
        void SpawnNewZavr(IBeing zavr, int speed, int sight, int newGeneration, Directions directions);
        Position GetPosition(IBeing being);
        void MoveZavr(Zavr zavr, Directions itemDirection, in int chosenSpeed);
    }
}
