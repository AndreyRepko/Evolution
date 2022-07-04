using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Windows.Controls;
using Evolution.Game.Model;
using Evolution.Game.Model.Items;
using Evolution.Game.Model.Positions;
using Moq;
using NUnit.Framework;

namespace EvolutionTest
{
    public class ZavrActionTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Must_ZavrEatIfCan()
        {
            /*//Arrange
            var position = new Position(10,10);
            var zavr = new Zavr(position, 1, true, 200, 1, Directions.Up, 5);

            var world = new Mock<IZavrWorldInteraction>();
            world.Setup(x => x.CanEat(position)).Returns(true);
            world.Setup(x => x.EatVegetable(position)).Returns((new Position(9, 9), 10));
            world.Setup(x => x.WhatZavrCanSee(It.IsAny<Position>(), It.IsAny<int>(), It.IsAny<Directions>()))
                .Returns(new SeenItems {(Directions.UpLeft, 10, new object())});

            //Act
            zavr.NextTurn(true, world.Object);

            //Assert
            Assert.AreEqual(new Position(9,9), zavr.Position);*/

        }
    }
}