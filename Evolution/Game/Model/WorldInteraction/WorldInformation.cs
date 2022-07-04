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
    public class WorldInformation : IVegetableWorldInteraction 
    {
        protected readonly GameRunner _world;

        public WorldInformation(GameRunner world)
        {
            _world = world;
        }

        protected List<(IBeing Being, Position Position)> GetWorldAround(Position position, int sight)
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

        public void MarkItemAsVictim(IVictim victim, object aggressor)
        {
            _world.AddAggression(aggressor, victim);
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
    }
}
