using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoreTextOutlines
{
    public class Preferences : INotifyPropertyChanged
    {
        private bool _isEnabled = true;
        private bool _isCustom = false;
        private int _red = 0;
        private int _green = 0;
        private int _blue = 0;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsCustom
        {
            get => _isCustom;
            set
            {
                _isCustom = value;
                OnPropertyChanged();
            }
        }

        public int Red
        {
            get => _red;
            set
            {
                _red = value;
                OnPropertyChanged();
            }
        }

        public int Green
        {
            get => _green;
            set
            {
                _green = value;
                OnPropertyChanged();
            }
        }

        public int Blue
        {
            get => _blue;
            set
            {
                _blue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
