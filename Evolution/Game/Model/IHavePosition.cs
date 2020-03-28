namespace Evolution.Game.Model
{
    public class PositionReadOnly
    {
        protected int _x;
        protected int _y;

        public int X => _x;

        public int Y => _y;
    }

    public class Position : PositionReadOnly
    {
        public new int X
        {
            get => _x;
            set => _x = value;
        }

        public new int Y
        {
            get => _y;
            set => _y = value;
        }
    }

    /// <summary>
    /// Position of the object
    /// </summary>
    public interface IHavePosition
    {
        PositionReadOnly Position { get; }
    }
}
