using HrtzSysInfo.Extensions;

namespace HrtzSysInfo.Models
{
    public class Gpu : ObservableObject
    {
        private int _identifier;
        private double _temperature;

        public int Identifier
        {
            get { return _identifier; }
            set { SetField(ref _identifier, value); }
        }

        public double Temperature
        {
            get { return _temperature; }
            set { SetField(ref _temperature, value); }
        }
    }
}
