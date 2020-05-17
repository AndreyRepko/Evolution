using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Evolution.Game.Model.Items;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model.WorldInteraction
{
    /// <summary>
    /// The main purpose of this class if to split responsibility of providing information.
    /// So animals (like Zavrs) can't (as they should not) access the whole world around them, but can ask
    /// specific questions and receive answers.
    /// There are a lot of possibilities to cheat for animals, but, well, why not? :)
    /// </summary>
    public class WorldInformation : IZavrWorldInteraction, IVegetableWorldInteraction 
    {
        private readonly GameRunner _world;

        public WorldInformation(GameRunner world)
        {
            _world = world;
        }

        private List<(IBeing Being, Position Position)> GetWorldAround(Position position, int sight)
        {
            var population = _world.GetPopulation(Math.Max(0, position.X - sight), Math.Min(_world.MaxX-1, position.X + sight),
                Math.Max(0, position.Y - sight), Math.Min(_world.MaxY-1, position.Y + sight));
            
            return population.ToList();
        }

        public void CorrectPositionToAllowed(ref Position newPosition)
        {
            if (newPosition.X < 0) newPosition.X = 0;
            if (newPosition.Y < 0) newPosition.Y = 0;
            if (newPosition.X >= _world.MaxX) newPosition.X = _world.MaxX - 1;
            if (newPosition.Y >= _world.MaxY) newPosition.Y = _world.MaxY - 1;
        }

        public SeenItems WhatZavrCanSee(IBeing zavr, int sight, Directions direction)
        {
            var position = _world.GetBeingPosition(zavr);

            var result = new SeenItems();
            var items = GetWorldAround(position, sight).Where(x => x.Being is IFood);
            //ToDo : add limitaiton of see

            foreach (var item in items)
                result.Add(new SeenItem
                {
                    Where = position.GetRelativePosition(item.Position),
                    Nutrition = ((IFood)item.Being).Nutrition,
                    Item = item.Being
                });

            return result;
        }

        public bool CanEat(IBeing zavr)
        {
            var position = _world.GetBeingPosition(zavr);

            return GetWorldAround(position, 1).Where(x=>x.Being is IFood)
                .OrderByDescending(x => ((IFood)x.Being).Nutrition).Any();
        }

        public int EatVegetable(IBeing zavr)
        {
            var position = _world.GetBeingPosition(zavr);

            var food = GetWorldAround(position, 1).Where(x => x.Being is IFood)
                .OrderByDescending(x => ((IFood)x.Being).Nutrition).First();
            _world.EatFoodByZavr(food.Being, zavr);
            return ((IFood)food.Being).Nutrition;
        }

        public void MarkItemAsVictim(IVictim victim, object aggressor)
        {
            _world.AddAggression(aggressor, victim);
        }

        public void MarkZavrAsDead(Zavr zavr)
        {
            _world.Remove(zavr);
        }

        public void SpawnNewZavr(IBeing zavr, int speed, int sight, int newGeneration, Directions directions)
        {
            var position = _world.GetBeingPosition(zavr);

            var newPosition = position.MoveInDirection(directions, RandomNumberGenerator.GetInt32(1, 2));
            CorrectPositionToAllowed(ref newPosition);

            _world.AddNewZavr(newPosition, speed, sight, newGeneration);
        }

        public void SpawnNewVegetable(IBeing being, Directions directions)
        {
            var position = _world.GetBeingPosition(being);

            var newPosition = position.MoveInDirection(directions, RandomNumberGenerator.GetInt32(1, 6));
            CorrectPositionToAllowed(ref newPosition);

            _world.AddNewVegetable(newPosition);
        }

        public Position GetPosition(IBeing being)
        {
            return _world.GetBeingPosition(being);
        }

        public void MoveZavr(Zavr zavr, Directions itemDirection, in int chosenSpeed)
        {
            var position = _world.GetBeingPosition(zavr);
            var newPosition = position.MoveInDirection(itemDirection, chosenSpeed);
            CorrectPositionToAllowed(ref newPosition);

            _world.TryToMoveZavr(zavr, newPosition);
        }
        /*
        public void SpawnNewZavr(IBeing zavr, int speed, int sight, int newGeneration, Directions directions)
        {
            throw new NotImplementedException();
        }
        */
    }
}
