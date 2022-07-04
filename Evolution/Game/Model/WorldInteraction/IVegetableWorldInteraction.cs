using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model.WorldInteraction
{
    public interface IVegetableWorldInteraction
    {
        void SpawnNewVegetable(IBeing being, Directions directions);
        Position GetPosition(IBeing being);
    }
}
