using GalaSoft.MvvmLight;

namespace WoodCutterCalculator.Models.Stock
{
    public class MinorElementOfStock : ObservableObject
    {
        #region Fields

        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private string _borderColor;
        private string _class;

        #endregion

        #region Properties

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                if (_x == value)
                {
                    return;
                }

                _x = value;
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                if (_y == value)
                {
                    return;
                }

                _y = value;
                RaisePropertyChanged();
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (_width == value)
                {
                    return;
                }

                _width = value;
                RaisePropertyChanged();
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (_height == value)
                {
                    return;
                }

                _height = value;
                RaisePropertyChanged();
            }
        }

        public string BorderColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
                if (_borderColor == value)
                {
                    return;
                }

                _borderColor = value;
                RaisePropertyChanged();
            }
        }

        public string Class
        {
            get
            {
                return _class;
            }

            set
            {
                if (_class == value)
                {
                    return;
                }

                _class = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public MinorElementOfStock(double y, double x, double width, double height, string borderColor, string @class)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            BorderColor = borderColor;
            Class = @class;
        }

        #endregion
    }
}
