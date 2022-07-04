namespace Evolution.Presenter.ChartControl
{
    public class DataPoint:NotifierBase
    {
        private double _day = new double();

        public DataPoint(double day, double value)
        {
            _day = day;
            _value = value;
        }

        public double Day
        {
            get { return _day; }
            set
            {
                SetProperty(ref _day, value);
            }
        }

        private double _value = new double();
        public double Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
            }
        }
    }
}
