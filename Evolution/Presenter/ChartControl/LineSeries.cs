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

        private string _caption = "";
        public string Caption
        {
            get { return _caption; }
            set
            {
                SetProperty(ref _caption, value);
            }
        }
    }
}
