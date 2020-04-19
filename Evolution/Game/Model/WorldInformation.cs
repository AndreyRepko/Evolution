using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model
{
    /// <summary>
    /// The main purpose of this class if to split responsibility of providing information.
    /// So animals (like Zavrs) can't (as they should not) access the whole world around them, but can ask
    /// specific questions and receive answers.
    /// There are a lot of possibilities to cheat for animals, but, well, why not? :)
    /// </summary>
    public class WorldInformation : IZavrWorldInteraction, IVegetetableWorldInteraction 
    {
        private readonly GameRunner _world;

        public WorldInformation(GameRunner world)
        {
            _world = world;
        }

        private List<IBeing> GetWorldAround(Position position, int sight)
        {
            var items = _world.Population.Where(b => position.IsPositionInSight(b.Position, sight)).ToList();
            return items;
        }

        public SeenItems WhatZavrCanSee(Position zavrPosition, int sight, Directions direction)
        {
            var result = new SeenItems();
            var items = GetWorldAround(zavrPosition, sight).OfType<EnergyBox>();
            //ToDo : add limitaiton of see

            foreach(var item in items)
                        result.Add((zavrPosition.GetRelativePosition(item.Position), item.Nutrition, item));

            return result;
        }

        public void CorrectPositionToAllowed(ref Position newPosition)
        {
            if (newPosition.X < 0) newPosition.X = 0;
            if (newPosition.Y < 0) newPosition.Y = 0;
            if (newPosition.X >= _world.MaxX) newPosition.X = _world.MaxX - 1;
            if (newPosition.Y >= _world.MaxY) newPosition.Y = _world.MaxY - 1;
        }

        public bool CanEat(Position position)
        {
            return GetWorldAround(position, 1).OfType<EnergyBox>()
                .OrderByDescending(x => x.Nutrition).Any();
        }

        public (Position position, int nutriotion) EatVegetable(Position position)
        {
            var food =  GetWorldAround(position, 1).OfType<EnergyBox>()
                .OrderByDescending(x => x.Nutrition).First();
            _world.Remove(food.Position);
            return (food.Position, food.Nutrition);
        }

        public void MarkItemAsVictim(object victim, object aggressor)
        {
            _world.AddAggression(aggressor, victim);
        }

        public void MarkZavrAsDead(Position position)
        {
            _world.Remove(position);   
        }

        public void SpawnNewVegetable(Position position, Directions directions)
        {
            var newPosition = position.MoveInDirection(directions, RandomNumberGenerator.GetInt32(1, 6));
            CorrectPositionToAllowed(ref newPosition);

            _world.AddNewVegetable(newPosition);
        }

        public void SpawnNewZavr(Position position, int speed, int sight, Directions directions)
        {
            var newPosition = position.MoveInDirection(directions, RandomNumberGenerator.GetInt32(1, 2));
            CorrectPositionToAllowed(ref newPosition);

            _world.AddNewZavr(newPosition, speed, sight);
        }
    }
}
