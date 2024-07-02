using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoreTextOptions
{
    public class Preferences : INotifyPropertyChanged
    {
        private bool _isCustomTextColor = false;
        private int _textRed = 255;
        private int _textGreen = 255;
        private int _textBlue = 255;
        private bool _isOutlineDisabled = false;
        private bool _isCustomOutline = false;
        private int _outlineRed = 0;
        private int _outlineGreen = 0;
        private int _outlineBlue = 0;

        public bool IsCustomTextColor
        {
            get => _isCustomTextColor;
            set
            {
                _isCustomTextColor = value;
                OnPropertyChanged();
            }
        }

        public int TextRed
        {
            get => _textRed;
            set
            {
                _textRed = value;
                OnPropertyChanged();
            }
        }

        public int TextGreen
        {
            get => _textGreen;
            set
            {
                _textGreen = value;
                OnPropertyChanged();
            }
        }

        public int TextBlue
        {
            get => _textBlue;
            set
            {
                _textBlue = value;
                OnPropertyChanged();
            }
        }

        public bool IsOutlineDisabled
        {
            get => _isOutlineDisabled;
            set
            {
                _isOutlineDisabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsCustomOutline
        {
            get => _isCustomOutline;
            set
            {
                _isCustomOutline = value;
                OnPropertyChanged();
            }
        }

        public int OutlineRed
        {
            get => _outlineRed;
            set
            {
                _outlineRed = value;
                OnPropertyChanged();
            }
        }

        public int OutlineGreen
        {
            get => _outlineGreen;
            set
            {
                _outlineGreen = value;
                OnPropertyChanged();
            }
        }

        public int OutlineBlue
        {
            get => _outlineBlue;
            set
            {
                _outlineBlue = value;
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
