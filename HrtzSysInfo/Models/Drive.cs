using HrtzSysInfo.Extensions;

namespace HrtzSysInfo.Models
{
    public class Drive : ObservableObject
    {
        private string _name;
        private long _totalSize;
        private long _availableFreeSpace;

        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public long TotalSize
        {
            get { return _totalSize; }
            set { SetField(ref _totalSize, value); }
        }

        public long AvailableFreeSpace
        {
            get { return _availableFreeSpace; }
            set { SetField(ref _availableFreeSpace, value); }
        }
    }
}
