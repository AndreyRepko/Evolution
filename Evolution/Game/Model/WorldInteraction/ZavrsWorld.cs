using System.Linq;
using System.Security.Cryptography;
using Evolution.Game.Model.Items;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model.WorldInteraction
{
    public class ZavrsWorld : WorldInformation, IZavrWorldInteraction
    {
        private Zavr _zavr;

        public ZavrsWorld(GameRunner world, Zavr zavr) : base(world)
        {
            _zavr = zavr;
        }

        public SeenItems WhatZavrCanSee(int sight, Directions direction)
        {
            var position = _world.GetBeingPosition(_zavr);

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

        public bool CanEat()
        {
            var position = _world.GetBeingPosition(_zavr);

            return GetWorldAround(position, 1).Where(x => x.Being is IFood)
                .OrderByDescending(x => ((IFood)x.Being).Nutrition).Any();
        }

        public int EatVegetable()
        {
            var position = _world.GetBeingPosition(_zavr);

            var food = GetWorldAround(position, 1).Where(x => x.Being is IFood)
                .OrderByDescending(x => ((IFood)x.Being).Nutrition).First();
            _world.EatFoodByZavr(food.Being, _zavr);
            return ((IFood)food.Being).Nutrition;
        }

        public void MarkZavrAsDead()
        {
            _world.Remove(_zavr);
        }

        public void SpawnNewZavr(int speed, int sight, int newGeneration, Directions directions)
        {
            var position = _world.GetBeingPosition(_zavr);

            var newPosition = position.MoveInDirection(directions, RandomNumberGenerator.GetInt32(1, 2));
            CorrectPositionToAllowed(ref newPosition);

            _world.AddNewZavr(newPosition, speed, sight, newGeneration);
        }

        public void MoveZavr(Directions itemDirection, in int chosenSpeed)
        {
            var position = _world.GetBeingPosition(_zavr);
            var newPosition = position.MoveInDirection(itemDirection, chosenSpeed);
            CorrectPositionToAllowed(ref newPosition);

            _world.TryToMoveZavr(_zavr, newPosition);
        }
    }
}
