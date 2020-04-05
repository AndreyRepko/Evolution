﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    /// <summary>
    /// Information about what Zavr actually can see in the world
    /// Please, take attention that exact distance and if food is aggressive is not available at the moment
    /// </summary>
    public class SeenItems : List<(Directions where, int nutrition)>
    { }

    public interface IZavrInformationProvider
    {
        /// <summary>
        /// What the zavr can see.
        /// </summary>
        /// <param name="zavrPosition">The zavr position.</param>
        /// <param name="sight">The sight distance.</param>
        /// <param name="direction">Direction of view of zavr</param>
        /// <returns></returns>
        SeenItems WhatZavrCanSee(PositionReadOnly zavrPosition, int sight, Directions direction);

        void CorrectPositionToAllowed(ref Position newPosition);
        bool CanEat(PositionReadOnly position);
        (PositionReadOnly position, int nutriotion) EatVegitable(PositionReadOnly position);
    }
}
