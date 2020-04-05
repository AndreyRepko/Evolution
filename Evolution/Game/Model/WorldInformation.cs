using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Game.Model
{
    /// <summary>
    /// The main purpose of this class if to split responsibility of providing information.
    /// So animals (like Zavrs) can't (as they should not) access the whole world around them, but can ask
    /// specific questions and receive answers.
    /// There are a lot of possibilities to cheat for animals, but, well, why not? :)
    /// </summary>
    public class WorldInformation : IZavrInformationProvider
    {
        private readonly GameRunner _world;

        public WorldInformation(GameRunner world)
        {
            _world = world;
        }

        private IEnumerable<IBeing> GetWorldAround(PositionReadOnly position, int sight)
        {
            for (var x = Math.Min(0, position.X - sight); x < Math.Min(_world.MaxX, position.X + sight); x++)
            for (var y = Math.Min(0, position.Y - sight); y < Math.Min(_world.MaxY, position.Y + sight); y++)
            {
                var item = _world.Population.FirstOrDefault(b => b.Position.X == x && b.Position.Y == y);

                yield return item;
            }
        }

        public SeenItems WhatZavrCanSee(PositionReadOnly zavrPosition, int sight, Directions direction)
        {
            var result = new SeenItems();
            var items = GetWorldAround(zavrPosition, sight).OfType<Vegetable>();
            //ToDo : add limitaiton of see

            foreach(var item in items)
                        result.Add((Position.GetRelativePosition(zavrPosition, item.Position.X, item.Position.Y), item.Nutrition));

            return result;
        }

        public void CorrectPositionToAllowed(ref Position newPosition)
        {
            if (newPosition.X < 0) newPosition.X = 0;
            if (newPosition.Y < 0) newPosition.Y = 0;
            if (newPosition.X > _world.MaxX) newPosition.X = _world.MaxX;
            if (newPosition.Y > _world.MaxY) newPosition.Y = _world.MaxY;
        }

        public bool CanEat(PositionReadOnly position)
        {
            return WhatZavrCanSee(position, 1, Directions.Right).OrderByDescending(x=>x.nutrition).Any();
        }

        public (PositionReadOnly position, int nutriotion) EatVegitable(PositionReadOnly position)
        {
            var food =  GetWorldAround(position, 1).OfType<Vegetable>()
                .OrderByDescending(x => x.Nutrition).First();
            _world.Remove(food);
            return (food.Position, food.Nutrition);
        }
    }
}
