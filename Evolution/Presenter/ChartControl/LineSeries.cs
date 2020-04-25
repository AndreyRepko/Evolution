using System.Collections.ObjectModel;

namespace Evolution.Presenter.ChartControl
{
    public class LineSeries:NotifierBase
    {
        private ObservableCollection<DataPoint> _data = new ObservableCollection<DataPoint>();
        public ObservableCollection<DataPoint> Data
        {
            get { return _data; }
            set
            {
                SetProperty(ref _data, value);
            }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }
}
